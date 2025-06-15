using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreMVC_SchoolSystem.ViewModels {
    public class LoginViewModel {
        [Required(ErrorMessage = "Přihlašovací jméno je povinné.")]

        [DisplayName("Uživatelské jméno")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Heslo je povinné.")]
        [DisplayName("Heslo")]
        
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        [DisplayName("Zapamatovat")]
        public bool Remember { get; set; }
    }
}
