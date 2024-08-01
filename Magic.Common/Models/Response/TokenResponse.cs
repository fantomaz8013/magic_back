namespace Magic.Common.Models.Response
{
    public class TokenResponse
    { 
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public DateTime Expires { get; set; }
        public DateTime ExpiresRefresh { get; set; }

        public string Role { get; set; }
        public Guid UserId { get; set; }
    }
}
