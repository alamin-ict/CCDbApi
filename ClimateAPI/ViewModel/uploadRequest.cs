namespace CCDbApi.ViewModel
{
    public class UploadMediaRequest
    {
        public IFormFile? File { get; set; }

        public string? MediaUrl { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }
    }
    public class UpdateMediaRequest
    {
        public string Id { get; set; }

        public IFormFile? File { get; set; }

        public string? MediaUrl { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }
    }
}
