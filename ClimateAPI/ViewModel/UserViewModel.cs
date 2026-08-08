using CCDbApi.Model;

namespace CCDbApi.ViewModel
{
    public class UserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public RoleType Role { get; set; } = RoleType.Admin;
        public string Password { get; set; }
    }
    public class UpdateUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public RoleType Role { get; set; } = RoleType.Admin;
        public UserStatus Status { get; set; }
        public string Password { get; set; }
    }
}
