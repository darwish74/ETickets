using ETickets.Repository.IRepository;
using System.ComponentModel.DataAnnotations;

namespace ETickets.Models
{
    public class UniqueNameAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var movieRepository = (IMovie)validationContext.GetService(typeof(IMovie));
            if (value == null)
            { 
                return null;
            }
            else 
            {
                string name = value.ToString();
                var _movie = movieRepository.GetOne(e => e.Name == name);
                if (_movie != null) 
                {
                    return new ValidationResult("This Name is Found Already");
                }
                return ValidationResult.Success;
            }
        }
    }
}
