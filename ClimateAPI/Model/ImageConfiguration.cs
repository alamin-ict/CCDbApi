
namespace CCDbApi.Model
{
    public class ImageConfiguration : BaseEntity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string ImagePosition { get; set; } //RequestCatalog, Social Impact, MemberShip
        public string DetailsText { get; set; }
        public string UserId {  get; set; } 
    }
}
