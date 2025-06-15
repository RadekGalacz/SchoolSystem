using System.ComponentModel;

namespace AspNetCoreMVC_SchoolSystem.DTO {
    public class StudentDTO {
        public int Id { get; set; }
        [DisplayName("Jméno")]
        public string FirstName { get; set; }
        [DisplayName("Příjmení")]
        public string LastName { get; set; }
        [DisplayName("Datum narození")]
        public DateOnly DateOfBirth { get; set; }
    }
}
