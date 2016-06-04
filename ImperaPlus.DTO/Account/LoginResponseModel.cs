using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperaPlus.DTO.Account
{
    public class LoginResponseModel
    {
        public string Access_token { get; set;}

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Language { get; set;}

        public string Roles { get; set; }
    }
}
