using System.ComponentModel.DataAnnotations;

namespace ETickets.Models.ViewModel
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
    }
}