namespace CRMTestAPI.Configuration
{
    public class AuthConfig
    {
        public string JwtSecretKey { get; set; }

        public int JwtExpirationInMonths { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        private string UploadsDirectory { get; set; }
    }
}