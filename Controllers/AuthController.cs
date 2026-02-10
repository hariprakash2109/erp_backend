using Microsoft.AspNetCore.Mvc;
using erp.Data;
using System.Linq;

namespace erp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Employees
                .FirstOrDefault(e => e.employee_id == request.employee_id
                                  && e.password == request.password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            return Ok("Login Successful");
        }
    }

    public class LoginRequest
    {
       
        public string employee_id { get; set; }
        public string password { get; set; }
    }
}
