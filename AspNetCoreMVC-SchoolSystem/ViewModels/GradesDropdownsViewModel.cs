using AspNetCoreMVC_SchoolSystem.Models;

namespace AspNetCoreMVC_SchoolSystem.ViewModels {
    public class GradesDropdownsViewModel {
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
    }
}
