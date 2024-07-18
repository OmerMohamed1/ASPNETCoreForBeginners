namespace ASPNETCoreForBeginners.Entities
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Lifetime { get; set; }
        public string SigningKey { get; set; }
        // public string EncryptionKey { get; set; }
    }

}
