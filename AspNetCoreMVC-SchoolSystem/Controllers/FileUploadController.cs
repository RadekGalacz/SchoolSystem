using AspNetCoreMVC_SchoolSystem.DTO;
using AspNetCoreMVC_SchoolSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml;

namespace AspNetCoreMVC_SchoolSystem.Controllers {
    public class FileUploadController : Controller {
        StudentService _studentService;

        public FileUploadController(StudentService studentService) {
            _studentService = studentService;
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file) {
            if (file == null || file.Length == 0) {
                return RedirectToAction("Fail");
            }

            string filePath = Path.GetFullPath(file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create)) {
                await file.CopyToAsync(stream);
                stream.Close();
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(filePath);
                XmlElement root = xmlDocument.DocumentElement;
                foreach(XmlNode node in root.SelectNodes("/Students/Student")) {
                    StudentDTO studentDTO = new StudentDTO() {
                        FirstName = node.ChildNodes[0].InnerText,
                        LastName = node.ChildNodes[1].InnerText,
                        DateOfBirth = DateOnly.Parse(node.ChildNodes[2].InnerText, CultureInfo.CreateSpecificCulture("cs-CZ"))
                        
                    };
                    await _studentService.CreateAsync(studentDTO);
                }
            }
            return RedirectToAction("Index", "Students"); 
        }
        public IActionResult Fail() {
            return View();
        }
    }
}
