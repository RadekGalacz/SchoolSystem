using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreMVC_SchoolSystem.ViewModels {
    public class UserViewModel {
        [DisplayName("Jméno")]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [DisplayName("Heslo")]
        public string Password { get; set; }
    }
}
