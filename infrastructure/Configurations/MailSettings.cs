namespace infrastructure.Configurations
{
    public class MailSettings
    {
    public string  Server { get; set; }
    public string Username { get; set; }
    public string  Password { get; set; }
        public string SenderName { get; set; }   
    public  int Port { get; set; }
    public bool TLS { get; set; }
    }
}
