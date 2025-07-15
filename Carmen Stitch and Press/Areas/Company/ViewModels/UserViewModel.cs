using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carmen_Stitch_and_Press.Areas.Company.ViewModels
{
    public class UserViewModel : IdentityUser
    {
   
        public string FirstName { get; set; }
 
        public string LastName { get; set; }

        [NotMapped]
        public string FullName 
        {
            get 
            {
                return $"{FirstName} {LastName}";
            }
        }

        [NotMapped]
        public string Role { get; set; } = string.Empty;
    }
}
