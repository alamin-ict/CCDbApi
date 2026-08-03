namespace CCDbApi.ViewModel
{
    public class TrainingInfoViewModel
    {
        public Guid? Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; } = string.Empty;
        public string CourseLink { get; set; }
        public string Venue { get; set; }
        public string? CourseOverview { get; set; }
        public string? CourseDescription { get; set; }
        public string? Register { get; set; }
        public string? RegisterOverview { get; set; }
        public string? TrainingLocation { get; set; }
        public string? Subject { get; set; }
        public int? CourseFee{ get; set; }
        public string? Date { get; set; }
    }
}
