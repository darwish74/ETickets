using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ETickets.Models.ViewModel
{
    [PrimaryKey("Name")]
    public class LoginVM
    {
        [Required(ErrorMessage="*")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name="Remember Me")]
        public bool RememberMe { get; set;}
    }
}
