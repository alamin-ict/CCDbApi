
namespace CCDbApi.Model
{
    public class Role:BaseEntity
    {
        public string Name {  get; set; } 
        public string? Type {  get; set; } //1,2,3
        public string UserId {  get; set; } 
    }
}
