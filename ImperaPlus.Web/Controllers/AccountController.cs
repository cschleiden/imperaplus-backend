using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using ImperaPlus.Application;
using ImperaPlus.Application.Users;
using ImperaPlus.Domain;
using ImperaPlus.DTO;
using ImperaPlus.DTO.Account;
using ImperaPlus.Web;
using ImperaPlus.Web.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog.Fluent;
using OpenIddict.Core;
using OpenIddict.Models;

namespace ImperaPlus.Backend.Controllers
{
    [Authorize]
    [Route("api/Account")]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(void), 200)]
    public class AccountController : Controller
    {
        public const string LocalLoginProvider = "Local";

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;


        private readonly IEmailService emailSender;
        private readonly Microsoft.Extensions.Logging.ILogger logger;
        private IUserService userService;

        public AccountController(
            OpenIddictApplicationManager<OpenIddictApplication> applicationManager,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailService emailSender,
            ILoggerFactory loggerFactory,
            IUserService userService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.logger = loggerFactory.CreateLogger<AccountController>();
            this.userService = userService;
        }

        [AllowAnonymous]
        [Consumes("application/x-www-form-urlencoded")]
        [HttpPost("token")]
        // [Swashbuckle.SwaggerGen.Annotations.SwaggerOperationFilter(typeof(FormFilter))]
        [ProducesResponseType(typeof(LoginResponseModel), 200)]
        public async Task<IActionResult> Exchange([FromForm] LoginRequest loginRequest)
        {
            var request = HttpContext.GetOpenIdConnectRequest();

            if (request.IsPasswordGrantType())
            {
                var user = await userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    return BadRequest(new ErrorResponse(Application.ErrorCode.UsernameOrPasswordNotCorrect, "The username/password is invalid."));
                }

                // Ensure the user is allowed to sign in.
                if (!await signInManager.CanSignInAsync(user))
                {
                    return BadRequest(new ErrorResponse(Application.ErrorCode.UsernameOrPasswordNotCorrect, "User cannot sign in."));
                }

                // Reject the token request if two-factor authentication has been enabled by the user.
                if (userManager.SupportsUserTwoFactor && await userManager.GetTwoFactorEnabledAsync(user))
                {
                    return BadRequest(new ErrorResponse(Application.ErrorCode.UsernameOrPasswordNotCorrect, "The username/password is invalid."));
                }

                // Ensure the user is not already locked out.
                if (userManager.SupportsUserLockout && await userManager.IsLockedOutAsync(user))
                {
                    return BadRequest(new ErrorResponse(Application.ErrorCode.AccountIsLocked, "This account is locked."));
                }

                // Ensure the password is valid.
                if (!await userManager.CheckPasswordAsync(user, request.Password))
                {
                    if (userManager.SupportsUserLockout)
                    {
                        await userManager.AccessFailedAsync(user);
                    }

                    return BadRequest(new ErrorResponse(Application.ErrorCode.UsernameOrPasswordNotCorrect, "The username/password is invalid."));
                }

                if (user.IsDeleted)
                {
                    return this.BadRequest(new ErrorResponse(Application.ErrorCode.AccountIsDeleted, "Account is deleted and will be removed soon"));
                }

                if (userManager.SupportsUserLockout)
                {
                    await userManager.ResetAccessFailedCountAsync(user);
                }

                // Store last login date
                this.userService.TrackLogin(user);

                // Create a new authentication ticket.
                var ticket = await CreateTicketAsync(request, user);
                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }
            else if (request.IsRefreshTokenGrantType())
            {
                // Retrieve the claims principal stored in the refresh token.
                var info = await HttpContext.Authentication.GetAuthenticateInfoAsync(OpenIdConnectServerDefaults.AuthenticationScheme);

                // Retrieve the user profile corresponding to the refresh token.
                var user = await userManager.GetUserAsync(info.Principal);
                if (user == null)
                {
                    return BadRequest(new ErrorResponse(Application.ErrorCode.UsernameOrPasswordNotCorrect, "Refresh token not valid."));
                }

                // Ensure the user is still allowed to sign in.
                if (!await signInManager.CanSignInAsync(user))
                {
                    return BadRequest(new ErrorResponse(Application.ErrorCode.UsernameOrPasswordNotCorrect, "User cannot sign in."));
                }

                // Create a new authentication ticket, but reuse the properties stored
                // in the refresh token, including the scopes originally granted.
                var ticket = await CreateTicketAsync(request, user, info.Properties);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            return BadRequest(new ErrorResponse(Application.ErrorCode.GenericApplicationError, "Grant type is not supported."));
        }

        private async Task<AuthenticationTicket> CreateTicketAsync(OpenIdConnectRequest request, User user, AuthenticationProperties properties = null)
        {
            // Set the list of scopes granted to the client application.
            // Note: the offline_access scope must be granted
            // to allow OpenIddict to return a refresh token.
            var scopes = new[] {
                OpenIdConnectConstants.Scopes.OpenId,
                OpenIdConnectConstants.Scopes.Email,
                OpenIdConnectConstants.Scopes.Profile,
                OpenIdConnectConstants.Scopes.OfflineAccess,
                OpenIdConnectConstants.Scopes.Profile,
                OpenIddictConstants.Scopes.Roles
            }.Intersect(request.GetScopes());

            // Create a new ClaimsPrincipal containing the claims that
            // will be used to create an id_token, a token or a code.
            var principal = await signInManager.CreateUserPrincipalAsync(user);

            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.
            foreach (var claim in principal.Claims)
            {
                // Always include the user identifier in the
                // access token and the identity token.
                if (claim.Type == OpenIdConnectConstants.Claims.Name)
                {
                    claim.SetDestinations(OpenIdConnectConstants.Destinations.AccessToken,
                                          OpenIdConnectConstants.Destinations.IdentityToken);
                }

                // Include the name claim, but only if the "profile" scope was requested.
                else if (claim.Type == OpenIdConnectConstants.Claims.Name && scopes.Contains(OpenIdConnectConstants.Scopes.Profile))
                {
                    claim.SetDestinations(OpenIdConnectConstants.Destinations.IdentityToken);
                }

                // Include the role claims, but only if the "roles" scope was requested.
                else if (claim.Type == OpenIdConnectConstants.Claims.Role && scopes.Contains(OpenIddictConstants.Scopes.Roles))
                {
                    claim.SetDestinations(OpenIdConnectConstants.Destinations.AccessToken,
                                          OpenIdConnectConstants.Destinations.IdentityToken);
                }

                // The other claims won't be added to the access
                // and identity tokens and will be kept private.
            }

            // Create a new authentication ticket holding the user identity.
            var ticket = new AuthenticationTicket(
                principal,
                properties,
                OpenIdConnectServerDefaults.AuthenticationScheme);

            if (!request.IsRefreshTokenGrantType())
            {
                ticket.SetScopes(scopes);
            }

            return ticket;
        }

        /// <summary>
        /// Checks if a username is available
        /// </summary>
        /// <param name="userName">Username to check</param>
        /// <returns>True if username is available</returns>
        [AllowAnonymous]
        [Route("UserNameAvailable")]
        [HttpGet]
        [ProducesResponseType(typeof(void), 200)]
        public async Task<IActionResult> GetUserNameAvailable([FromQuery] string userName)
        {
            if (string.IsNullOrEmpty(userName) || userName.Length < 4)
            {
                return this.BadRequest();
            }

            var user = await this.userManager.FindByNameAsync(userName);

            return this.Ok(user == null);
        }

        /// <summary>
        /// Get user information
        /// </summary>
        /// <returns></returns>
        [Route("UserInfo")]
        [HttpGet]
        [ProducesResponseType(typeof(DTO.Account.UserInfo), 200)]
        public async Task<IActionResult> GetUserInfo()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.BadRequest(
                    new ErrorResponse(Application.ErrorCode.UserIdNotFound.ToString(), string.Empty));
            }

            var roles = await this.userManager.GetRolesAsync(user);
            var logins = await this.userManager.GetLoginsAsync(user);

            return this.Ok(new DTO.Account.UserInfo
            {
                UserId = user.Id,
                UserName = user.UserName,
                HasRegistered = logins.Any(x => x.LoginProvider == "Local"),
                LoginProvider = null, // TODO : CS: 
                Language = user.Language,
                Roles = roles.ToArray(),
                AllianceAdmin = user.AllianceId.HasValue && user.AllianceId != Guid.Empty && user.IsAllianceAdmin, // Can only be admin if in an alliance
                AllianceId = user.AllianceId
            });
        }

        /// <summary>
        /// Get user information for an external user (i.e., just logged in using an external provider)
        /// </summary>
        /// <returns></returns>
        [Route("ExternalUserInfo")]
        [HttpGet]
        [ProducesResponseType(typeof(DTO.Account.UserInfo), 200)]
        public async Task<IActionResult> GetExternalUserInfo()
        {
            var user = await this.GetCurrentUserAsync();
            var externalLogin = await this.signInManager.GetExternalLoginInfoAsync();

            return this.Ok(new DTO.Account.UserInfo
            {
                UserId = user.Id,
                UserName = user.UserName,
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            });
        }

        // POST api/Account/Logout
        [Route("Logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        [HttpGet]
        [ProducesResponseType(typeof(ManageInfoViewModel), 200)]
        public async Task<IActionResult> GetManageInfo([FromQuery] string returnUrl, [FromQuery] bool generateState = false)
        {
            User user = await this.GetCurrentUserAsync();
            if (user == null)
            {
                return this.BadRequest();
            }

            var logins = new List<UserLoginInfoViewModel>();

            foreach (var linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return this.Ok(new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                UserName = user.UserName,
                Logins = logins.ToArray(),
                ExternalLoginProviders = this.GetExternalLogins().ToArray()
            });
        }

        [Route("ChangePassword")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordBindingModel model)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return this.BadRequest();
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                return BadRequest(new ErrorResponse(Application.ErrorCode.PasswordsDoNotMatch, "Passwords do not match."));
            }

            var result = await this.userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
            }

            return this.CheckResult(result);
        }

        [Route("SetPassword")]
        [HttpPost]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordBindingModel model)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return this.BadRequest();
            }

            var result = await userManager.AddPasswordAsync(user, model.NewPassword);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
            }

            return this.CheckResult(result);
        }

        [Route("Delete")]
        [HttpPost]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountBindingModel model)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return this.BadRequest();
            }

            if (!await this.userManager.CheckPasswordAsync(user, model.Password))
            {
                return this.BadRequest(new ErrorResponse(Application.ErrorCode.UsernameOrPasswordNotCorrect, "Password is not correct."));
            }

            this.userService.DeleteAccount();
            return this.Ok();
        }

        [Route("Language")]
        [HttpPatch]        
        public async Task<IActionResult> SetLanguage(LanguageModel model)
        {
            // await this._userManager.SetLanguageAsync(this.User.Identity.GetUserId(), model.Language);

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        /*[Route("AddExternalLogin")]
        [HttpPost]        
        public async Task<IActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {           
            this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest(Application.ErrorCode.ExternalLoginFailure.ToString());
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest(Application.ErrorCode.UserWithExternalLoginExists.ToString());
            }

            IdentityResult result = await _userManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            this.CheckResult(result, null);

            return Ok();
        }*/

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        [HttpPost]       
        public async Task<IActionResult> RemoveLogin([FromBody] RemoveLoginBindingModel model)
        {
            var user = await this.GetCurrentUserAsync();
            if (user == null)
            {
                return this.BadRequest();
            }

            IdentityResult result;
            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await userManager.RemovePasswordAsync(user);                
            }
            else
            {
                result = await userManager.RemoveLoginAsync(
                    user,
                    model.LoginProvider, 
                    model.ProviderKey);
            }

            if (result.Succeeded)
            {
                return this.Ok();
            }

            return this.CheckResult(result);
        }

        // GET api/Account/ExternalLogin
        //[OverrideAuthentication]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        /*[AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        [HttpGet]
        public async Task<IActionResult> GetExternalLogin(string provider)
        {
            // If not authenticated, redirect to provider            
            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            var externalLogin = await this._signInManager.GetExternalLoginInfoAsync();
            if (externalLogin == null)
            {               
                return this.BadRequest();
            }

            if (externalLogin.LoginProvider != provider)
            {
                // Redirect to correct provider
                this._signInManager.SignOutAsync()
                this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            User user = await _userManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                ClaimsIdentity oAuthIdentity = await _userManager.CreateIdentityAsync(user,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await _userManager.CreateIdentityAsync(user,
                    CookieAuthenticationDefaults.AuthenticationType);
                AuthenticationProperties properties = await ApplicationOAuthProvider.CreateProperties(_userManager, user);
                this.AuthenticationManager.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims(); // .Claims;
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                this.AuthenticationManager.SignIn(identity);
            }

            return Ok();
        }*/

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExternalLoginViewModel>), 200)]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins()
        {
            var descriptions = this.signInManager.GetExternalAuthenticationSchemes();

            return descriptions
                .Select(description => new ExternalLoginViewModel
                {
                    Name = description.DisplayName,
                    AuthenticationScheme = description.AuthenticationScheme
                })
                .ToList();
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        [HttpPost]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterBindingModel model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                Language = model.Language
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (Startup.RequireUserConfirmation)
                {
                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    await this.sendEmailConfirmation(user, code, model.Language, model.CallbackUrl);
                }
            }

            return this.CheckResult(result);
        }
        
        /// <summary>
        /// Resend the email confirmation account to the given user account
        /// </summary>
        [AllowAnonymous]
        [Route("ResendConfirmation")]
        [HttpPost]
        public async Task<IActionResult> ResendConfirmationCode([FromBody] ResendConfirmationModel model)
        {
            var user = await this.userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return this.BadRequest(new ErrorResponse(Application.ErrorCode.UsernameOrPasswordNotCorrect, "Username or password are not correct."));
            }

            if (!await this.userManager.IsEmailConfirmedAsync(user))
            {
                string code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                await this.sendEmailConfirmation(user, code, model.Language, model.CallbackUrl);
            }

            return this.Ok();
        }

        private async Task sendEmailConfirmation(User user, string code, string language, string callbackUrl)
        {
            // Create email confirmation link
            callbackUrl = callbackUrl.Replace("userId", user.Id);
            callbackUrl = callbackUrl.Replace("code", WebUtility.UrlEncode(code));

            this.SetCulture(language);

            string body = string.Format(Resources.EmailConfirmationBody, callbackUrl);
            await this.emailSender.SendMail(user.Email, Resources.EmailConfirmationSubject, body, body);
        }

        /// <summary>
        /// Confirm user account using code provided in mail
        /// </summary>
        /// <param name="model">Model containing id and code</param>
        /// <returns>Success if successfully activated</returns>
        [AllowAnonymous]
        [Route("ConfirmEmail")]
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmationModel model)
        {
            Log.Info().Message($"Confirming email for user {model.UserId} - {model.Code}").Write();

            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                Log.Error().Message($"Could not find user {model.UserId}").Write();

                return this.BadRequest();
            }

            var result = await userManager.ConfirmEmailAsync(user, model.Code);

            return this.CheckResult(result);
        }
        
        /// <summary>
        /// Request password reset link
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("ForgotPassword")]
        [HttpPost]        
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null 
                || !(await userManager.IsEmailConfirmedAsync(user))
                || !string.Equals(user.Email, model.Email, StringComparison.InvariantCultureIgnoreCase))
            {
                return this.Ok();
            }

            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            
            var callbackUrl = model.CallbackUrl;
            callbackUrl = callbackUrl.Replace("userId", user.Id);
            callbackUrl = callbackUrl.Replace("code", WebUtility.UrlEncode(code));

            this.SetCulture(model.Language);

            await this.emailSender.SendMail(
                user.Email, 
                Resources.ResetPasswordSubject, 
                string.Format(Resources.ResetPasswordBody, callbackUrl));

            return this.Ok();
        }

        /// <summary>
        /// Reset password confirmation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ResetPassword")]
        [AllowAnonymous]        
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return this.Ok();
            }

            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
            
            return this.CheckResult(result);
        }

        // POST api/Account/RegisterExternal
        /// <summary>
        /// Create user accout for an external login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[OverrideAuthentication]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        [HttpPost]        
        public async Task<IActionResult> RegisterExternal([FromBody] RegisterExternalBindingModel model)
        {
            var externalLoginInfo = await this.signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                return this.BadRequest();
            }

            var user = new User
            {
                UserName = model.UserName,
                Email = "", // TOOD: CS: Get email from claims
                EmailConfirmed = true // External accounts are trusted by default
            };

            var result = await this.userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await this.userManager.AddLoginAsync(user, externalLoginInfo);
                if (result.Succeeded)
                {
                    await this.signInManager.SignInAsync(user, isPersistent: false);
                    return this.Ok();
                }
            }

            return this.CheckResult(result);
        }

        #region Helpers        

        private IActionResult CheckResult(IdentityResult result)
        {
            if (result == null)
            {
                // No error code
                return BadRequest();
            }            
            
            if (!result.Succeeded)
            {                
                var errors = result.Errors.Select(x => this.TransformError(x.Code));

                var error = new ErrorResponse(errors.First().Item1, errors.First().Item2.ToString());

                error.Parameter_Errors = errors
                    .GroupBy(x => x.Item1, x => x.Item2)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.ToString())
                    .ToArray());

                return this.BadRequest(error);
            }

            return this.Ok();
        }

        private Tuple<string, Application.ErrorCode> TransformError(string error)
        {
            switch (error)
            {
                case nameof(IdentityErrorDescriber.DuplicateEmail):
                    return Tuple.Create("Email", Application.ErrorCode.EmailAlreadyInUse);

                case nameof(IdentityErrorDescriber.InvalidEmail):
                    return Tuple.Create("Email", Application.ErrorCode.EmailInvalid);

                case nameof(IdentityErrorDescriber.DuplicateUserName):
                    return Tuple.Create("Username", Application.ErrorCode.UsernameAlreadyInUse);

                case nameof(IdentityErrorDescriber.LoginAlreadyAssociated):
                    return Tuple.Create("Login", Application.ErrorCode.UserWithExternalLoginExists);

                case nameof(IdentityErrorDescriber.PasswordMismatch):
                    return Tuple.Create("Password", Application.ErrorCode.UsernameOrPasswordNotCorrect);

                case nameof(IdentityErrorDescriber.PasswordRequiresDigit):
                case nameof(IdentityErrorDescriber.PasswordRequiresLower):
                case nameof(IdentityErrorDescriber.PasswordRequiresNonAlphanumeric):
                case nameof(IdentityErrorDescriber.PasswordRequiresUpper):
                case nameof(IdentityErrorDescriber.PasswordTooShort):
                    return Tuple.Create("Password", Application.ErrorCode.PasswordInvalid);

                case nameof(IdentityErrorDescriber.InvalidUserName):
                    return Tuple.Create("Username", Application.ErrorCode.UsernameInvalid);
            }

            // Default
            return Tuple.Create("General", Application.ErrorCode.GenericApplicationError);
        }

        private void SetCulture(string language)
        {
            var cultureInfo = CultureInfo.GetCultureInfo(language);

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        private Task<User> GetCurrentUserAsync()
        {
            return this.userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}
