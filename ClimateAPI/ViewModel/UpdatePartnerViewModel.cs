namespace CCDbApi.ViewModel
{
    public class UpdatePartnerViewModel
    {
      
        public string Id { get; set; }
        public string? Heading { get; set; }
        public string? SubTitle { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }//Team,client,partner,advisor,activity etc.
        public string DetailsLink {  get; set; }
        public string? Area { get; set; }
        public string? Category { get; set; }
    }
}
