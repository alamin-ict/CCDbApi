namespace CCDbApi.ViewModel
{
    public class SmtpSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
    }
    public class EmailViewModel
    {
        public string Content { get; set; }
       
        public string Subject { get; set; }
    }
}
