
namespace CCDbApi.Model
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public RoleType Type { get; set; } //1,2,3
       
    }
    public enum RoleType
    {
        Admin = 1,
        User = 2,
        SuperAdmin = 3,
        Other = 4
    }
}
