using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using ImperaPlus.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using ImperaPlus.Backend.Providers;
using ImperaPlus.Backend.Results;
using ImperaPlus.Backend.Identity;
using ImperaPlus.DTO;
using System.Globalization;
using System.Threading;
using ImperaPlus.Backend.Resources;
using ImperaPlus.DTO.Account;

namespace ImperaPlus.Backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        /// <summary>
        /// Name for the local provider 
        /// </summary>
        private const string LocalLoginProvider = "Local";

        public AccountController(ApplicationUserManager userManager)
            : this(userManager, Authentication.OAuthOptions.AccessTokenFormat)
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager { get; private set; }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        /// <summary>
        /// Checks if a username is available
        /// </summary>
        /// <param name="userName">Username to check</param>
        /// <returns>True if username is available</returns>
        [AllowAnonymous]
        [Route("UserNameAvailable")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserNameAvailable(string userName)
        {
            if (userName.Length < 4)
            {
                return this.BadRequest();
            }

            var user = await this.UserManager.FindByNameAsync(userName);

            return this.Ok(user == null);
        }

        /// <summary>
        /// Get user information
        /// </summary>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        [HttpGet]
        [ResponseType(typeof(DTO.Account.UserInfo))]
        public async Task<IHttpActionResult> GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            var userId = this.User.Identity.GetUserId();            
            var user = await this.UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new HttpResponseException(this.Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    new ErrorResponse(Application.ErrorCode.UserIdNotFound.ToString(), string.Empty)));
            }

            var roles = await this.UserManager.GetRolesAsync(user.Id);

            return this.Ok(new DTO.Account.UserInfo
            {
                UserId = User.Identity.GetUserId(),
                UserName = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null,

                Language = user != null ? user.Language : null,
                Roles = roles.ToArray()
            });
        }

        /// <summary>
        /// Get user information for an external user (i.e., just logged in using an external provider)
        /// </summary>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("ExternalUserInfo")]
        [HttpGet]
        [ResponseType(typeof(DTO.Account.UserInfo))]
        public IHttpActionResult GetExternalUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return this.Ok(new DTO.Account.UserInfo
            {
                UserId = User.Identity.GetUserId(),
                UserName = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            });
        }

        // POST api/Account/Logout
        [Route("Logout")]
        [HttpPost]
        public IHttpActionResult Logout()
        {
            this.AuthenticationManager.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        [HttpGet]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return null;
            }

            var logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
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

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                UserName = user.UserName,
                Logins = logins.ToArray(),
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState).ToArray()
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        [HttpPost]        
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            IdentityResult result = await UserManager.ChangePasswordAsync(this.User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);


            this.CheckResult(result, null);

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        [HttpPost]       
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            this.CheckResult(result, null);

            return Ok();
        }

        [Route("Language")]
        [HttpPatch]        
        public async Task<IHttpActionResult> SetLanguage(LanguageModel model)
        {
            await this.UserManager.SetLanguageAsync(this.User.Identity.GetUserId(), model.Language);

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        [HttpPost]        
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
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

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            this.CheckResult(result, null);

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        [HttpPost]       
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            this.CheckResult(result, null);

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        [HttpGet]
        public async Task<IHttpActionResult> GetExternalLogin(string provider)
        {
            // If not authenticated, redirect to provider            
            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            var externalLogin2 = await this.AuthenticationManager.GetExternalLoginInfoAsync();
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
            if (externalLogin == null)
            {
                // TODO: CS: Warp in specific handler
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                // Redirect to correct provider
                this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            User user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                ClaimsIdentity oAuthIdentity = await UserManager.CreateIdentityAsync(user,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await UserManager.CreateIdentityAsync(user,
                    CookieAuthenticationDefaults.AuthenticationType);
                AuthenticationProperties properties = await ApplicationOAuthProvider.CreateProperties(UserManager, user);
                this.AuthenticationManager.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims(); // .Claims;
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                this.AuthenticationManager.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        [HttpGet]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = this.AuthenticationManager.GetExternalAuthenticationTypes();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            return descriptions.Select(description => new ExternalLoginViewModel
            {
                Name = description.Caption,
                Url = this.Url.Route("ExternalLogin", new
                {
                    provider = description.AuthenticationType,
                    response_type = "token",
                    client_id = Authentication.PublicClientId,
                    redirect_uri = new Uri(this.Request.RequestUri, returnUrl).AbsoluteUri,
                    state = state
                }),
                State = state
            }).ToList();
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        [HttpPost]        
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                Language = model.Language
            };

            IdentityResult result = await this.UserManager.CreateAsync(user, model.Password);

            this.CheckResult(result, user);

            if (Startup.RequireUserConfirmation)
            {
                await this.sendEmailConfirmation(user, model.Language, model.CallbackUrl);
            }

            return Ok();
        }
        
        /// <summary>
        /// Resend the email confirmation account to the given user account
        /// </summary>
        [AllowAnonymous]
        [Route("ResendConfirmation")]
        [HttpPost]
        public async Task<IHttpActionResult> ResendConfirmationCode(ResendConfirmationModel model)
        {
            var user = await this.UserManager.FindAsync(model.UserName, model.Password);
            if (user == null)
            {
                var error = new ErrorResponse(Application.ErrorCode.UsernameOrPasswordNotCorrect, "Username or password are not correct.");
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.BadRequest, error));
            }

            if (!await this.UserManager.IsEmailConfirmedAsync(user.Id))
            {
                await this.sendEmailConfirmation(user, model.Language, model.CallbackUrl);
            }

            return this.Ok();                                
        }

        private async Task sendEmailConfirmation(User user, string language, string callbackUrl)
        {
            // Create email confirmation link
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            callbackUrl = callbackUrl.Replace("userId", user.Id);
            callbackUrl = callbackUrl.Replace("code", HttpUtility.UrlEncode(code));

            this.SetCulture(language);

            await UserManager.SendEmailAsync(user.Id,
                Resources.Resources.EmailConfirmationSubject,
                string.Format(Resources.Resources.EmailConfirmationBody, callbackUrl));
        }

        /// <summary>
        /// Confirm user account using code provided in mail
        /// </summary>
        /// <param name="model">Model containing id and code</param>
        /// <returns>Success if successfully activated</returns>
        [AllowAnonymous]
        [Route("ConfirmEmail")]
        [HttpPost]
        public async Task<IHttpActionResult> ConfirmEmail(ConfirmationModel model)
        {
            var result = await this.UserManager.ConfirmEmailAsync(model.UserId, model.Code);

            this.CheckResult(result, null);

            return this.Ok();
        }
        
        /// <summary>
        /// Request password reset link
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("ForgotPassword")]
        [HttpPost]        
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
            {
                // Do not give information that user doesn't exist
                return this.Ok();
            }

            var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            
            var callbackUrl = model.CallbackUrl;
            callbackUrl = callbackUrl.Replace("userId", user.Id);
            callbackUrl = callbackUrl.Replace("code", HttpUtility.UrlEncode(code));

            this.SetCulture(model.Language);

            await UserManager.SendEmailAsync(
                user.Id, 
                Resources.Resources.ResetPasswordSubject,
                string.Format(Resources.Resources.ResetPasswordBody, callbackUrl));

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
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordViewModel model)
        {     
            var user = await UserManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return this.Ok();
            }

            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);

            this.CheckResult(result, user);

            return this.Ok();
        }

        // POST api/Account/RegisterExternal
        /// <summary>
        /// Create user accout for an external login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        [HttpPost]        
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            var user = new User
            {
                UserName = model.UserName,
                Email = externalLogin.Email,
                EmailConfirmed = true // External accounts are trusted by default
            };

            user.Logins.Add(new IdentityUserLogin
            {
                LoginProvider = externalLogin.LoginProvider,
                ProviderKey = externalLogin.ProviderKey,
                UserId = user.Id // Why do we have to set explicitly?
            });

            IdentityResult result = await UserManager.CreateAsync(user);

            this.CheckResult(result, user);

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserManager.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager AuthenticationManager
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private void CheckResult(IdentityResult result, User user)
        {
            if (result == null)
            {
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.BadRequest));
            }            
            
            if (!result.Succeeded)
            {                
                var errors = result.Errors.Select(x => this.TransformError(x, user));

                var error = new ErrorResponse(errors.First().Item1.ToString(), "An error occured");

                error.Parameter_Errors = errors.GroupBy(x => x.Item1, x => x.Item2).ToDictionary(x => x.Key, x => x.Select(y => y.ToString()).ToArray());

                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.BadRequest, error));
            }
        }

        private Tuple<string, Application.ErrorCode> TransformError(string error, User user)
        {
            if (error == "User already in role.") return Tuple.Create("User", Application.ErrorCode.UserAlreadyInRole);
            else if (error == "User is not in role.") return Tuple.Create("User", Application.ErrorCode.UserNotInRole);
            //else if (error == "Role {0} does not exist.") return Tuple.Create( "De rol bestaat nog niet";
            //else if (error == "Store does not implement IUserClaimStore&lt;TUser&gt;.") return Tuple.Create( "";
            //else if (error == "No IUserTwoFactorProvider for '{0}' is registered.") return Tuple.Create( "";
            //else if (error == "Store does not implement IUserEmailStore&lt;TUser&gt;.") return Tuple.Create( "";
            else if (error == "Incorrect password.") return Tuple.Create("Password", Application.ErrorCode.UsernameOrPasswordNotCorrect);
            //else if (error == "Store does not implement IUserLockoutStore&lt;TUser&gt;.") return Tuple.Create( "";
            //else if (error == "No IUserTokenProvider is registered.") return Tuple.Create( "";
            //else if (error == "Store does not implement IUserRoleStore&lt;TUser&gt;.") return Tuple.Create( "";
            //else if (error == "Store does not implement IUserLoginStore&lt;TUser&gt;.") return Tuple.Create( "";
            else if (error == string.Format("User name {0} is invalid, can only contain letters or digits.", user.UserName)) return Tuple.Create("Username", Application.ErrorCode.UsernameInvalid);
            //else if (error == "Store does not implement IUserPhoneNumberStore&lt;TUser&gt;.") return Tuple.Create( "";
            //else if (error == "Store does not implement IUserConfirmationStore&lt;TUser&gt;.") return Tuple.Create( "";            
            //else if (error == "{0} cannot be null or empty.") return Tuple.Create( "";
            else if (user != null && error == "Name " + user.UserName + " is already taken.") return Tuple.Create("Username", Application.ErrorCode.UsernameAlreadyInUse);
            //else if (error == "User already has a password set.") return Tuple.Create( "Deze gebruiker heeft reeds een wachtwoord ingesteld.";
            //else if (error == "Store does not implement IUserPasswordStore&lt;TUser&gt;.") return Tuple.Create( "";            
            else if (error == "UserId not found.") return Tuple.Create("User", Application.ErrorCode.UserDoesNotExist);
            else if (error == "Invalid token.") return Tuple.Create("Token", Application.ErrorCode.InvalidToken);
            else if (user != null && error == "Email '" + user.Email + "' is invalid.") return Tuple.Create("Email", Application.ErrorCode.EmailInvalid);
            else if (user != null && error == "User " + user.UserName + " does not exist.") return Tuple.Create("Username", Application.ErrorCode.UserDoesNotExist);
            //else if (error == "Store does not implement IQueryableRoleStore&lt;TRole&gt;.") return Tuple.Create( "";
            //else if (error == "Lockout is not enabled for this user.") return Tuple.Create( "Lockout is niet geactiveerd voor deze gebruiker.";
            //else if (error == "Store does not implement IUserTwoFactorStore&lt;TUser&gt;.") return Tuple.Create( "";
            else if (error.StartsWith("Passwords must be at least ")) return Tuple.Create("Password", Application.ErrorCode.PasswordInvalid);
            else if (error == "Passwords must have at least one non letter or digit character.") return Tuple.Create("Password", Application.ErrorCode.PasswordInvalid);
            else if (error == "Passwords must have at least one uppercase ('A'-'Z').") return Tuple.Create("Password", Application.ErrorCode.PasswordInvalid);
            else if (error == "Passwords must have at least one digit ('0'-'9').") return Tuple.Create("Password", Application.ErrorCode.PasswordInvalid);
            else if (error == "Passwords must have at least one lowercase ('a'-'z').") return Tuple.Create("Password", Application.ErrorCode.PasswordInvalid);
            //else if (error == "Store does not implement IQueryableUserStore&lt;TUser&gt;.") return Tuple.Create( "";
            else if (user != null && error == "Email '" + user.Email + "' is already taken.") return Tuple.Create("Email", Application.ErrorCode.EmailAlreadyInUse);
            //else if (error == "Store does not implement IUserSecurityStampStore&lt;TUser&gt;.") return Tuple.Create( "";
            else if (error == "A user with that external login already exists.") return Tuple.Create("Login", Application.ErrorCode.UserWithExternalLoginExists);
            
            // Default
            return Tuple.Create("General", Application.ErrorCode.GenericApplicationError);
        }

        private void SetCulture(string language)
        {
            var cultureInfo = CultureInfo.GetCultureInfo(language);

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }
            public string Email { get; private set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, this.UserName, null, this.LoginProvider));
                }

                if (Email != null)
                {
                    claims.Add(new Claim(ClaimTypes.Email, this.Email, null, this.LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name),
                    Email = identity.FindFirstValue(ClaimTypes.Email)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static readonly RandomNumberGenerator Random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                Random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
