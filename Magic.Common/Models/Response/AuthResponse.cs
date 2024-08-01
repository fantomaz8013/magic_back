namespace Magic.Common.Models.Response
{
    public class AuthResponse
    {
        public TokenResponse? TokenResult { get; set; }
        public bool? IsNeedEnterCode { get; set; }
        public int ConfirmCodeLifeTime { get; set; }
    }
}
