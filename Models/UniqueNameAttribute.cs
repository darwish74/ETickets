using System.ComponentModel.DataAnnotations;

namespace ETickets.Models
{
    public class UniqueNameAttribute:ValidationAttribute
    {  
        
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null)
                return null;
            else (value != null)
            {
                string name = value.ToString();
                
                return 
            }
        }
    }
}
