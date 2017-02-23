namespace Experion.Marina.Core
{
    public class EmailParameter
    {
        public string AccountAddress { get; set; } = string.Empty;
        public string AccountPassword { get; set; } = string.Empty;
        public bool IsAuthenticationEnabled { get; set; } = false;
        public bool IsSslEnabled { get; set; } = false;
        public string PreferredEncoding { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; } = 25;
    }
}