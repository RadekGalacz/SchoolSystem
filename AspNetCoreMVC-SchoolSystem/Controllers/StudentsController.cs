using AspNetCoreMVC_SchoolSystem.DTO;
using AspNetCoreMVC_SchoolSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMVC_SchoolSystem.Controllers {
    [Authorize]
    public class StudentsController : Controller {
        StudentService _studentService;

        public StudentsController(StudentService studentService) {
            _studentService = studentService;
        }
        [HttpGet]
        public IActionResult Index() {
            var allStudents = _studentService.GetAll();
            return View(allStudents);
        }
        [HttpGet]
        [Authorize(Roles = "Teacher, Admin")]
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<IActionResult> CreateAsync(StudentDTO newStudent) {
            await _studentService.CreateAsync(newStudent);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> EditAsync(int id) {
            var studentToEdit = await _studentService.GetByIdAsync(id);
            return View(studentToEdit);
        }
        [HttpPost]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<IActionResult> EditAsync(StudentDTO StudentDTO, int id) {
            await _studentService.UpdateAsync(StudentDTO, id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize(Roles = "Teacher, Admin")]
        public async Task<IActionResult> Delete(int id) {
            await _studentService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
        public IActionResult Search (string q) {
            if (q == null || q.Length == 0) {
                return RedirectToAction("Index");
            }
            var foundStudents = _studentService.GetByName(q);
            return View("Index", foundStudents);
        }
        public async Task<IActionResult> GetToDelete(int id) {
            var studentDetails = await _studentService.GetByIdAsync(id);
            return View(studentDetails);
        }
    }
}
