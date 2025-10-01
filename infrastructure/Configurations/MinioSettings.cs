namespace infrastructure.Configurations
{
    public class MinioSettings
    {
        public string Endpoint { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string BucketName { get; set; } = string.Empty;
        public bool UseSSL { get; set; } = false;
        public int PresignedUrlExpiry { get; set; } = 604800; // 7 days in seconds
    }
}
