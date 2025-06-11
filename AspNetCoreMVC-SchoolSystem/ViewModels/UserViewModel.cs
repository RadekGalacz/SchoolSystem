using System.ComponentModel.DataAnnotations;

namespace AspNetCoreMVC_SchoolSystem.ViewModels {
    public class UserViewModel {
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
