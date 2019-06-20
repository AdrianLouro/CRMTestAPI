namespace CRMTestAPI
{
    public class AppConfig
    {
        public string JwtSecretKey { get; set; }

        public int JwtExpirationInMonths { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
