using Microsoft.AspNetCore.Identity;
namespace Turbo_Food_Main.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
       
    }
}
