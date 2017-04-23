namespace ImperaPlus.DTO.Account
{
    public class ExternalLoginViewModel
    {
        /// <summary>
        /// Name of external login provider
        /// </summary>
        public string Name { get; set; }

        public string AuthenticationScheme { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string UserName { get; set; }

        public UserLoginInfoViewModel[] Logins { get; set; }

        public ExternalLoginViewModel[] ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
}
