using System;

namespace DreamInMars.Dto
{
    public class AuthenticatedToken
    {
        public DateTime Expires { get; set; }
        public string Token { get; set; }
        public bool IsAuthenticated { get; set; }
        public AccountInfo Account { get; set; }
    }
}
