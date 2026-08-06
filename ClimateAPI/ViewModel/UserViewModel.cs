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
}
