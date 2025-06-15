using AspNetCoreMVC_SchoolSystem.DTO;
using AspNetCoreMVC_SchoolSystem.Models;

namespace AspNetCoreMVC_SchoolSystem.Services {
    public class StudentService {
        ApplicationDbContext _dbContext;

        public StudentService(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public IEnumerable<StudentDTO> GetAll() {
            var allStudents = _dbContext.Students.ToList();
            var studentDtos = new List<StudentDTO>();
            foreach (var student in allStudents) {
                StudentDTO studentDTO = ModelToDto(student);
                studentDtos.Add(studentDTO);
            }
            return studentDtos;
        }

        internal async Task CreateAsync(StudentDTO newStudent) {
            Student studentToSave = DtoToModel(newStudent);
            await _dbContext.Students.AddAsync(studentToSave);
            await _dbContext.SaveChangesAsync();
        }

        internal async Task<StudentDTO> GetByIdAsync(int id) {
            var studentToEdit = await _dbContext.Students.FindAsync(id);
            return ModelToDto(studentToEdit);
        }

        internal async Task UpdateAsync(StudentDTO studentDTO, int id) {
            _dbContext.Update(DtoToModel(studentDTO));
            await _dbContext.SaveChangesAsync();
        }

        private StudentDTO ModelToDto(Student? student) {
            return new StudentDTO() {
                DateOfBirth = student.DateOfBirth,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Id = student.Id
            };
        }
        private Student DtoToModel(StudentDTO newStudent) {
            return new Student {
                DateOfBirth = newStudent.DateOfBirth,
                FirstName = newStudent.FirstName,
                LastName = newStudent.LastName,
                Id = newStudent.Id
            };
        }

        internal async Task DeleteAsync(int id) {
            var studentToDelete = await _dbContext.Students.FindAsync(id);
            if (studentToDelete != null) {
                _dbContext.Students.Remove(studentToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }

        internal IEnumerable<StudentDTO> GetByName(string q) {
            var nameParts = q.Split(", ");
            if (nameParts.Length != 2)
                return Enumerable.Empty<StudentDTO>();

            string lastName = nameParts[0];
            string firstName = nameParts[1];

            var studentsThatMatch = _dbContext.Students
                .Where(st => st.LastName == lastName && st.FirstName == firstName);

            List<StudentDTO> returnedStudents = new List<StudentDTO>();

            foreach (var student in studentsThatMatch) {
                returnedStudents.Add(ModelToDto(student));
            }
            return returnedStudents;
        }
    }
}