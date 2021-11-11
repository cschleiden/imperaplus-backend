namespace ImperaPlus.DTO.Account
{
    public class LoginRequest
    {
        public string grant_type { get; set; }

        public string username { get; set; }

        public string password { get; set; }

        public string scope { get; set; }

        public string refresh_token { get; set; }
    }
}
