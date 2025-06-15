using AspNetCoreMVC_SchoolSystem.Models;
using System.ComponentModel;

namespace AspNetCoreMVC_SchoolSystem.DTO {
    public class GradeDTO {
        public int Id { get; set; }
        [DisplayName("Student")]
        public int StudentId { get; set; }
        [DisplayName("Předmět")]
        public int SubjectId { get; set; }
        [DisplayName("Celé jméno")]
        public string StudentFullName { get; set; }
        [DisplayName("Předmět")]
        public string SubjectName { get; set; }
        [DisplayName("Téma")]
        public string Topic { get; set; }
        [DisplayName("Známka")]
        public int Mark { get; set; }
        public DateTime Date { get; set; }
    }
}
