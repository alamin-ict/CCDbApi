using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CCDbApi.Model
{
    public class Subscribe
    {
        public Guid Id { get; set; }    
        public string Email {  get; set; }  
        public string? Source { get; set; } 
        public string? Category {  get; set; }  
        public DateTime? CreatedAt { get; set; }  
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } 
        public bool IsAtive {  get; set; }  
    }
}
