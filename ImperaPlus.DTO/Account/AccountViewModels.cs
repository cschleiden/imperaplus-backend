using System;
using System.Collections.Generic;

namespace ImperaPlus.DTO.Account
{
    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
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
