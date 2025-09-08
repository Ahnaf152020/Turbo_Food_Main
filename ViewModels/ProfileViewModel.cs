using System.ComponentModel.DataAnnotations;

namespace Turbo_Food_Main.ViewModels
{
    public class ProfileViewModel
    {
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
