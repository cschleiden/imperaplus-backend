using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ImperaPlus.Application;
using ImperaPlus.Application.Users;
using ImperaPlus.Domain;
using ImperaPlus.DTO;
using ImperaPlus.DTO.Account;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog.Fluent;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using OpenIddict.Server.AspNetCore;
using ErrorCode = ImperaPlus.Application.ErrorCode;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ImperaPlus.Web.Controllers
{
    [Authorize]
    [Route("Account")]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(void), 200)]
    public class AccountController : Controller
    {
        public const string LocalLoginProvider = "Local";


        private readonly IEmailService emailSender;
        private readonly ILogger logger;
        private readonly SignInManager<User> signInManager;

        private readonly UserManager<User> userManager;
        private readonly IUserService userService;

        public AccountController(
            OpenIddictApplicationManager<OpenIddictEntityFrameworkCoreApplication> applicationManager,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailService emailSender,
            ILoggerFactory loggerFactory,
            IUserService userService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            logger = loggerFactory.CreateLogger<AccountController>();
            this.userService = userService;
        }

        [AllowAnonymous]
        [Consumes("application/x-www-form-urlencoded")]
        [HttpPost("token")]
        // [Swashbuckle.SwaggerGen.Annotations.SwaggerOperationFilter(typeof(FormFilter))]
        [ProducesResponseType(typeof(LoginResponseModel), 200)]
        public async Task<IActionResult> Exchange([FromForm] LoginRequest loginRequest)
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                          throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            if (request.IsPasswordGrantType())
            {
                var user = await userManager.FindByNameAsync(request.Username);
                if (user == null)
                {
                    return BadRequest(new ErrorResponse(ErrorCode.UsernameOrPasswordNotCorrect,
                        "The username/password is invalid."));
                }

                // Ensure the user is allowed to sign in.
                if (!await signInManager.CanSignInAsync(user))
                {
                    return BadRequest(new ErrorResponse(ErrorCode.UsernameOrPasswordNotCorrect,
                        "User cannot sign in."));
                }

                // Reject the token request if two-factor authentication has been enabled by the user.
                if (userManager.SupportsUserTwoFactor && await userManager.GetTwoFactorEnabledAsync(user))
                {
                    return BadRequest(new ErrorResponse(ErrorCode.UsernameOrPasswordNotCorrect,
                        "The username/password is invalid."));
                }

                // Ensure the user is not already locked out.
                if (userManager.SupportsUserLockout && await userManager.IsLockedOutAsync(user))
                {
                    return BadRequest(new ErrorResponse(ErrorCode.AccountIsLocked, "This account is locked."));
                }

                // Ensure the password is valid.
                if (!await userManager.CheckPasswordAsync(user, request.Password))
                {
                    if (userManager.SupportsUserLockout)
                    {
                        await userManager.AccessFailedAsync(user);
                    }

                    return BadRequest(new ErrorResponse(ErrorCode.UsernameOrPasswordNotCorrect,
                        "The username/password is invalid."));
                }

                if (user.IsDeleted)
                {
                    return BadRequest(new ErrorResponse(ErrorCode.AccountIsDeleted,
                        "Account is deleted and will be removed soon"));
                }

                if (userManager.SupportsUserLockout)
                {
                    await userManager.ResetAccessFailedCountAsync(user);
                }

                // Store last login date
                userService.TrackLogin(user);

                // Create a new authentication ticket.
                var principal = await CreatePrincipalAsync(request, user);
                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            if (request.IsRefreshTokenGrantType())
            {
                // Retrieve the claims principal stored in the refresh token.
                var info = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                // Retrieve the user profile corresponding to the refresh token.
                var user = await userManager.GetUserAsync(info.Principal);
                if (user == null)
                {
                    return BadRequest(new ErrorResponse(ErrorCode.UsernameOrPasswordNotCorrect,
                        "Refresh token not valid."));
                }

                // Ensure the user is still allowed to sign in.
                if (!await signInManager.CanSignInAsync(user))
                {
                    return BadRequest(new ErrorResponse(ErrorCode.UsernameOrPasswordNotCorrect,
                        "User cannot sign in."));
                }

                var principal = await CreatePrincipalAsync(request, user);
                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            return BadRequest(new ErrorResponse(ErrorCode.GenericApplicationError, "Grant type is not supported."));
        }

        private async Task<ClaimsPrincipal> CreatePrincipalAsync(OpenIddictRequest request, User user)
        {
            // Create a new ClaimsPrincipal containing the claims that
            // will be used to create an id_token, a token or a code.
            var principal = await signInManager.CreateUserPrincipalAsync(user);

            // Set the list of scopes granted to the client application.
            // Note: the offline_access scope must be granted
            // to allow OpenIddict to return a refresh token.
            var scopes = new[]
            {
                OpenIddictConstants.Scopes.OpenId, OpenIddictConstants.Scopes.Email,
                OpenIddictConstants.Scopes.Profile, OpenIddictConstants.Scopes.OfflineAccess,
                OpenIddictConstants.Scopes.Roles
            }.Intersect(request.GetScopes()).ToHashSet();

            principal.SetScopes(scopes);

            foreach (var claim in principal.Claims)
            {
                claim.SetDestinations(GetDestinations(claim, principal));
            }

            return principal;
        }

        private IEnumerable<string> GetDestinations(Claim claim, ClaimsPrincipal principal)
        {
            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.

            switch (claim.Type)
            {
                case OpenIddictConstants.Claims.Name:
                    yield return OpenIddictConstants.Destinations.AccessToken;

                    if (principal.HasScope(OpenIddictConstants.Permissions.Scopes.Profile))
                    {
                        yield return OpenIddictConstants.Destinations.IdentityToken;
                    }

                    yield break;

                case OpenIddictConstants.Claims.Email:
                    yield return OpenIddictConstants.Destinations.AccessToken;

                    if (principal.HasScope(OpenIddictConstants.Permissions.Scopes.Email))
                    {
                        yield return OpenIddictConstants.Destinations.IdentityToken;
                    }

                    yield break;

                case OpenIddictConstants.Claims.Role:
                    yield return OpenIddictConstants.Destinations.AccessToken;

                    if (principal.HasScope(OpenIddictConstants.Permissions.Scopes.Roles))
                    {
                        yield return OpenIddictConstants.Destinations.IdentityToken;
                    }

                    yield break;

                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                case "AspNet.Identity.SecurityStamp": yield break;

                default:
                    yield return OpenIddictConstants.Destinations.AccessToken;
                    yield break;
            }
        }

        /// <summary>
        ///     Checks if a username is available
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
                return BadRequest();
            }

            var user = await userManager.FindByNameAsync(userName);

            return Ok(user == null);
        }

        /// <summary>
        ///     Get user information
        /// </summary>
        /// <returns></returns>
        [Route("UserInfo")]
        [HttpGet]
        [ProducesResponseType(typeof(UserInfo), 200)]
        public async Task<IActionResult> GetUserInfo()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(
                    new ErrorResponse(ErrorCode.UserIdNotFound.ToString(), string.Empty));
            }

            var roles = await userManager.GetRolesAsync(user);
            var logins = await userManager.GetLoginsAsync(user);

            return Ok(new UserInfo
            {
                UserId = user.Id,
                UserName = user.UserName,
                HasRegistered = logins.Any(x => x.LoginProvider == "Local"),
                LoginProvider = null,
                Language = user.Language,
                Roles = roles.ToArray(),
                AllianceAdmin =
                    user.AllianceId.HasValue && user.AllianceId != Guid.Empty &&
                    user.IsAllianceAdmin, // Can only be admin if in an alliance
                AllianceId = user.AllianceId
            });
        }

        // POST Account/Logout
        [Route("Logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }

        // GET Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        [HttpGet]
        [ProducesResponseType(typeof(ManageInfoViewModel), 200)]
        public async Task<IActionResult> GetManageInfo([FromQuery] string returnUrl,
            [FromQuery] bool generateState = false)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return BadRequest();
            }

            var logins = new List<UserLoginInfoViewModel>();

            foreach (var linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider, ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider, ProviderKey = user.UserName
                });
            }

            return Ok(new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                UserName = user.UserName,
                Logins = logins.ToArray(),
                ExternalLoginProviders = (await GetExternalLogins()).ToArray()
            });
        }

        [Route("ChangePassword")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordBindingModel model)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return BadRequest();
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                return BadRequest(new ErrorResponse(ErrorCode.PasswordsDoNotMatch, "Passwords do not match."));
            }

            var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, false);
            }

            return CheckResult(result);
        }

        [Route("SetPassword")]
        [HttpPost]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordBindingModel model)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return BadRequest();
            }

            var result = await userManager.AddPasswordAsync(user, model.NewPassword);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, false);
            }

            return CheckResult(result);
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
                return BadRequest();
            }

            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                return BadRequest(new ErrorResponse(ErrorCode.UsernameOrPasswordNotCorrect,
                    "Password is not correct."));
            }

            userService.DeleteAccount();
            return Ok();
        }

        [Route("Language")]
        [HttpPatch]
        public async Task<IActionResult> SetLanguage(LanguageModel model)
        {
            var user = await GetCurrentUserAsync();
            userService.SetLanguage(user, model.Language);

            return Ok();
        }

        // POST Account/RemoveLogin
        [Route("RemoveLogin")]
        [HttpPost]
        public async Task<IActionResult> RemoveLogin([FromBody] RemoveLoginBindingModel model)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return BadRequest();
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
                return Ok();
            }

            return CheckResult(result);
        }

        // GET Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExternalLoginViewModel>), 200)]
        public async Task<IEnumerable<ExternalLoginViewModel>> GetExternalLogins()
        {
            var descriptions = await signInManager.GetExternalAuthenticationSchemesAsync();

            return descriptions
                .Select(description => new ExternalLoginViewModel
                {
                    Name = description.DisplayName, AuthenticationScheme = description.Name
                })
                .ToList();
        }

        // POST Account/Register
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
                Language = model.Language,
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (Startup.RequireUserConfirmation)
                {
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    await sendEmailConfirmation(user, code, model.Language, model.CallbackUrl);
                }
            }

            return CheckResult(result);
        }

        /// <summary>
        ///     Resend the email confirmation account to the given user account
        /// </summary>
        [AllowAnonymous]
        [Route("ResendConfirmation")]
        [HttpPost]
        public async Task<IActionResult> ResendConfirmationCode([FromBody] ResendConfirmationModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return BadRequest(new ErrorResponse(ErrorCode.UsernameOrPasswordNotCorrect,
                    "Username or password are not correct."));
            }

            if (!await userManager.IsEmailConfirmedAsync(user))
            {
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                await sendEmailConfirmation(user, code, model.Language, model.CallbackUrl);
            }

            return Ok();
        }

        private async Task sendEmailConfirmation(User user, string code, string language, string callbackUrl)
        {
            // Create email confirmation link
            callbackUrl = callbackUrl.Replace("userId", user.Id);
            callbackUrl = callbackUrl.Replace("code", WebUtility.UrlEncode(code));

            SetCulture(language);

            var body = string.Format(Resources.Resources.EmailConfirmationBody, callbackUrl);
            await emailSender.SendMail(user.Email, Resources.Resources.EmailConfirmationSubject, body, body);
        }

        /// <summary>
        ///     Confirm user account using code provided in mail
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

                return BadRequest();
            }

            var result = await userManager.ConfirmEmailAsync(user, model.Code);

            return CheckResult(result);
        }

        /// <summary>
        ///     Request password reset link
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
                || !await userManager.IsEmailConfirmedAsync(user)
                || !string.Equals(user.Email, model.Email, StringComparison.InvariantCultureIgnoreCase))
            {
                return Ok();
            }

            var code = await userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = model.CallbackUrl;
            callbackUrl = callbackUrl.Replace("userId", user.Id);
            callbackUrl = callbackUrl.Replace("code", WebUtility.UrlEncode(code));

            SetCulture(model.Language);

            await emailSender.SendMail(
                user.Email,
                Resources.Resources.ResetPasswordSubject,
                string.Format(Resources.Resources.ResetPasswordBody, callbackUrl));

            return Ok();
        }

        /// <summary>
        ///     Reset password confirmation
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
                return Ok();
            }

            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);

            return CheckResult(result);
        }

        // POST Account/RegisterExternal
        /// <summary>
        ///     Create user account for an external login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[OverrideAuthentication]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        [HttpPost]
        public async Task<IActionResult> RegisterExternal([FromBody] RegisterExternalBindingModel model)
        {
            var externalLoginInfo = await signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                return BadRequest();
            }

            var user = new User
            {
                UserName = model.UserName,
                Email = "", // TOOD: CS: Get email from claims
                EmailConfirmed = true // External accounts are trusted by default
            };

            var result = await userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await userManager.AddLoginAsync(user, externalLoginInfo);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return Ok();
                }
            }

            return CheckResult(result);
        }

        #region Helpers

        private IActionResult CheckResult(IdentityResult result)
        {
            if (result == null)
                // No error code
            {
                return BadRequest();
            }

            if (!result.Succeeded)
            {
                Log.Error().Message($"Error: {string.Join(" ", result.Errors.Select(x => x.Description))}").Write();

                var errors = result.Errors.Select(x => TransformError(x.Code));

                var error = new ErrorResponse(errors.First().Item1, errors.First().Item2.ToString());

                error.Parameter_Errors = errors
                    .GroupBy(x => x.Item1, x => x.Item2)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.ToString())
                        .ToArray());

                return BadRequest(error);
            }

            return Ok();
        }

        private Tuple<string, ErrorCode> TransformError(string error)
        {
            switch (error)
            {
                case nameof(IdentityErrorDescriber.DuplicateEmail):
                    return Tuple.Create("Email", ErrorCode.EmailAlreadyInUse);

                case nameof(IdentityErrorDescriber.InvalidEmail):
                    return Tuple.Create("Email", ErrorCode.EmailInvalid);

                case nameof(IdentityErrorDescriber.DuplicateUserName):
                    return Tuple.Create("Username", ErrorCode.UsernameAlreadyInUse);

                case nameof(IdentityErrorDescriber.LoginAlreadyAssociated):
                    return Tuple.Create("Login", ErrorCode.UserWithExternalLoginExists);

                case nameof(IdentityErrorDescriber.PasswordMismatch):
                    return Tuple.Create("Password", ErrorCode.UsernameOrPasswordNotCorrect);

                case nameof(IdentityErrorDescriber.PasswordRequiresDigit):
                case nameof(IdentityErrorDescriber.PasswordRequiresLower):
                case nameof(IdentityErrorDescriber.PasswordRequiresNonAlphanumeric):
                case nameof(IdentityErrorDescriber.PasswordRequiresUpper):
                case nameof(IdentityErrorDescriber.PasswordTooShort):
                    return Tuple.Create("Password", ErrorCode.PasswordInvalid);

                case nameof(IdentityErrorDescriber.InvalidUserName):
                    return Tuple.Create("Username", ErrorCode.UsernameInvalid);
            }

            // Default
            return Tuple.Create("General", ErrorCode.GenericApplicationError);
        }

        private void SetCulture(string language)
        {
            var cultureInfo = CultureInfo.GetCultureInfo(language);

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        private Task<User> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}
