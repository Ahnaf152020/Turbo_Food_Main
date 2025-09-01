using System.ComponentModel.DataAnnotations;

namespace Turbo_Food_Main.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

    }
}
