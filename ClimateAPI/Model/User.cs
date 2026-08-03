
namespace CCDbApi.Model
{
    public class User:BaseEntity
    {
        public string UserName { get; set; }    
        public string Email {  get; set; }  
        public string UserRole {  get; set; }   
        public string Password {  get; set; }
      
    }
}
