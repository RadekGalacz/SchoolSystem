using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMVC_SchoolSystem.Controllers {
    [Route("health")]
    public class HealthController : Controller {
        private readonly ApplicationDbContext _context;
        public HealthController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Index() {
            try {
                await _context.Database.ExecuteSqlRawAsync("SELECT 1");
                return Ok("Healthy");
            }
            catch (Exception ex) {
                return StatusCode(500, $"DB Error: {ex.Message}");
            }
        }
    }
}