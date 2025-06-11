using AspNetCoreMVC_SchoolSystem.Models;
using System.ComponentModel;

namespace AspNetCoreMVC_SchoolSystem.DTO {
    public class GradeDTO {
        public int Id { get; set; }
        [DisplayName("Student")]
        public int StudentId { get; set; }
        [DisplayName("Subject")]
        public int SubjectId { get; set; }
        public string StudentFullName { get; set; }
        public string SubjectName { get; set; }
        public string Topic { get; set; }
        public int Mark { get; set; }
        public DateTime Date { get; set; }
    }
}
