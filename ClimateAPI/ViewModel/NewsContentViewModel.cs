namespace CCDbApi.ViewModel
{
    public class NewsContentViewModel
    {
        public string Type { get; set; }//pagecontent or newscontent
        public string Headline { get; set; }
        public string ImageUrl { get; set; }
        public string MainText { get; set; }
        public string DetailText { get; set; }
        public string? Subscribed { get; set; }
        // public int Order { get; set; }

    }
}
