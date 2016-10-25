using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperaPlus.DTO.Account
{
    public class LoginRequest
    {
        public string grant_type { get; set; }

        public string username { get; set; }

        public string password { get; set; }
    }
}
