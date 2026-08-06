using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CCDbApi.Model
{
    public class Subscribe:BaseEntity
    {
        
        public string Email {  get; set; }  
        public string? Source { get; set; } 
        public string? Category {  get; set; }  
        
    }
}
