using AspNetCoreMVC_SchoolSystem.DTO;
using AspNetCoreMVC_SchoolSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMVC_SchoolSystem.Controllers {
    [Authorize]
    public class SubjectController : Controller {
        SubjectService _subjectService;

        public SubjectController(SubjectService subjectService) {
            _subjectService = subjectService;
        }
        [HttpGet]
        public IActionResult Index() {
            var allSubjects = _subjectService.GetAll();
            return View(allSubjects);
        }
        [HttpGet]
        [Authorize(Roles = "Teacher, Admin")]
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<IActionResult> CreateAsync(SubjectDTO newSubject) {
            if (!ModelState.IsValid) {
                return View(newSubject );
            }
            await _subjectService.CreateAsync(newSubject);
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<IActionResult> EditAsync(int id) {
                var subjectToEdit = await _subjectService.GetByIdAsync(id); 
            if (subjectToEdit == null) {
                return View("NotFound");
            }
            return View(subjectToEdit);
        }
        [HttpPost]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<IActionResult> EditAsync(SubjectDTO SubjectDTO, int id) {
            await _subjectService.UpdateAsync(SubjectDTO, id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize(Roles = "Headmaster, Admin")]
        public async Task<IActionResult> Delete(int id) {
            await _subjectService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> GetToDelete(int id) {
            var studentDetails = await _subjectService.GetByIdAsync(id);
            return View(studentDetails);
        }
    }
}
