
namespace CCDbApi.Model
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string RoleId { get; set; }
        public UserStatus Status { get; set; }
        public string Password { get; set; }

    }
    public enum UserStatus
    {
        Pending=1,
        Active=2,
        Inactive=3,
        Disabled=4
    }
}
