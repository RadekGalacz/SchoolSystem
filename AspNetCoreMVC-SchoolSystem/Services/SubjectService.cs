using AspNetCoreMVC_SchoolSystem.DTO;
using AspNetCoreMVC_SchoolSystem.Models;

namespace AspNetCoreMVC_SchoolSystem.Services {
    public class SubjectService {
        ApplicationDbContext _dbContext;
        public SubjectService(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }
        public IEnumerable<SubjectDTO> GetAll() {
            var allSubjects = _dbContext.Subjects.ToList();
            var subjectDtos = new List<SubjectDTO>();
            foreach (var subj in allSubjects) {
                SubjectDTO subjectDTO = ModelToDto(subj);
                subjectDtos.Add(subjectDTO);
            }
            return subjectDtos;
        }
        internal async Task CreateAsync(SubjectDTO newSubject) {
            Subject subjectToSave = DtoToModel(newSubject);
            await _dbContext.Subjects.AddAsync(subjectToSave);
            await _dbContext.SaveChangesAsync();
        }
        internal async Task<SubjectDTO> GetByIdAsync(int id) {
            var subjectToEdit = await _dbContext.Subjects.FindAsync(id);
            if (subjectToEdit == null) {
                return null;
            }
            return ModelToDto(subjectToEdit);
        }

        internal async Task UpdateAsync(SubjectDTO subjectDTO, int id) {
            _dbContext.Update(DtoToModel(subjectDTO));
            await _dbContext.SaveChangesAsync();
        }
        internal async Task DeleteAsync(int id) {
            var subjectToDelete = await _dbContext.Subjects.FindAsync(id);
            if (subjectToDelete != null) {
                _dbContext.Subjects.Remove(subjectToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }
        private SubjectDTO ModelToDto(Subject subj) {
            return new SubjectDTO {
                Id = subj.Id,
                Name = subj.Name
            };
        }
        private Subject DtoToModel(SubjectDTO newSubject) {
            return new Subject {
                Id = newSubject.Id,
                Name = newSubject.Name,
            };            
        }
    }
}
