using System.ComponentModel.DataAnnotations;

namespace AspNetCoreMVC_SchoolSystem.DTO {
    public class SubjectDTO {
        public int Id { get; set; }
        //[MinLength(2), MaxLength(20)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Zadejte nejméně 2 a nejvíce 50 znaků")]
        public string Name { get; set; }
    }
}
