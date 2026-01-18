namespace AuthAPI.Settings
{
    public class AuthSettings
    {
        public string SecretKeyPath { get; set; }
        public string AuthTokenExpireDays { get; set; }
        public string RefreshTokenExpireDays { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }

    }
}
