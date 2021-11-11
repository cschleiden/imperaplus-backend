using System.ComponentModel.DataAnnotations;

namespace ImperaPlus.DTO.Account
{
    public class AddExternalLoginBindingModel
    {
        [Required] public string ExternalAccessToken { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)] public string ConfirmPassword { get; set; }
    }

    public class DeleteAccountBindingModel
    {
        /// <summary>
        /// Current password of user account to delete
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class ResetPasswordViewModel
    {
        /// <summary>
        /// Id of user
        /// </summary>
        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)] public string ConfirmPassword { get; set; }

        /// <summary>
        /// Password reset confirmation code
        /// </summary>
        [Required]
        public string Code { get; set; }
    }

    public class RegisterBindingModel
    {
        /// <summary>
        /// Name to register
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Password to use for login
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Password confirmation
        /// </summary>
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Email for new user
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Language to set for new user
        /// </summary>
        [Required]
        [StringLength(2)]
        public string Language { get; set; }

        /// <summary>
        /// Callback for email confirmation link. Tokens userId and code will be replaced.
        /// </summary>
        [Required]
        public string CallbackUrl { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Callback url to include in email for confirmation link
        /// </summary>
        [Required]
        public string CallbackUrl { get; set; }

        [Required] public string UserName { get; set; }

        /// <summary>
        /// Email address of user to request password for
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Language to send confirmation mail in
        /// </summary>
        [Required]
        [StringLength(2)]
        public string Language { get; set; }
    }

    public class ResendConfirmationModel
    {
        /// <summary>
        /// Callback url to include in email for confirmation link
        /// </summary>
        [Required]
        public string CallbackUrl { get; set; }

        /// <summary>
        /// Username to resend confirmation for
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Password for user
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Language to send confirmation mail in
        /// </summary>
        [Required]
        [StringLength(2)]
        public string Language { get; set; }
    }

    public class ConfirmationModel
    {
        /// <summary>
        /// Id of user to confirm
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// Confirmation code
        /// </summary>
        [Required]
        public string Code { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        [Required] public string UserName { get; set; }

        [Required] [EmailAddress] public string Email { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required] public string LoginProvider { get; set; }

        [Required] public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        // TODO: CS: Localize
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        // TODO: CS: Localize
        [DataType(DataType.Password)] public string ConfirmPassword { get; set; }
    }

    public class LanguageModel
    {
        /// <summary>
        /// Language code to set for user
        /// </summary>
        [Required]
        [StringLength(2)]
        public string Language { get; set; }
    }
}
