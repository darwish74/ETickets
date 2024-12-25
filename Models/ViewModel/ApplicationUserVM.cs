using System.ComponentModel.DataAnnotations;

namespace ETickets.Models.ViewModel
{
    public class ApplicationUserVM
    {
        public int Id { get; set; }
      
        [Required]
        public string UserName { get; set; }
       
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
      
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
      
        [Required]
        public string Name { get; set; }
       
        [Required]
        public string Address { get; set; }
    }
}
