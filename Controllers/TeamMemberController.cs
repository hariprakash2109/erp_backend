using Microsoft.AspNetCore.Mvc;
using erp.Data;
using erp.Models;
using erp.Services;

namespace erp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMemberController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public TeamMemberController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost("save")]
        public IActionResult Save([FromBody] TeamMember member)
        {
            try
            {
                if (member.DateOfBirth.HasValue)
                {
                    member.DateOfBirth = DateTime.SpecifyKind(
                        member.DateOfBirth.Value,
                        DateTimeKind.Utc
                    );
                }

                if (member.DateOfJoining.HasValue)
                {
                    member.DateOfJoining = DateTime.SpecifyKind(
                        member.DateOfJoining.Value,
                        DateTimeKind.Utc
                    );
                }

                _context.TeamMembers.Add(member);
                _context.SaveChanges();

                try
                {
                     string emailBody = GenerateWelcomeEmail(
                member.FirstName,
                member.EmployeeCode
            );

            _emailService.SendEmail(
                member.Email,
                "Welcome to Our Organization",
                emailBody
            );
                }
                catch (Exception emailEx)
                {
                    return Ok("Saved, but Email failed: " + emailEx.Message);
                }

                return Ok("Saved & Email Sent");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }
        [HttpPost("update-password")]
        public IActionResult UpdatePassword([FromBody] PasswordUpdateRequest request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // 1. Update the 'team_members' table
                    var member = _context.TeamMembers.FirstOrDefault(m => m.EmployeeCode == request.EmployeeCode);
                    if (member == null)
                        return NotFound(new { message = "Employee code not found." });

                    member.Password = request.Password;

                    // 2. Synchronize with the 'employee' table
                    var empLogin = _context.Employees.FirstOrDefault(e => e.employee_id == request.EmployeeCode);

                    if (empLogin != null)
                    {
                        // Update existing record
                        empLogin.password = request.Password;
                    }
                    else
                    {
                        // Create new login record if it doesn't exist
                        var newLogin = new Employee
                        {
                            employee_id = request.EmployeeCode,
                            password = request.Password
                        };
                        _context.Employees.Add(newLogin);
                    }

                    _context.SaveChanges();
                    transaction.Commit();

                    return Ok(new { message = "Password updated in both tables successfully!" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode(500, new { message = "Error syncing tables: " + ex.Message });
                }
            }
        }

        // DTO for the request
        public class PasswordUpdateRequest
        {
            public string EmployeeCode { get; set; }
            public string Password { get; set; }
        }
        private string GenerateWelcomeEmail(string? name, string? employeeCode)
        {
            string setupLink = $"http://localhost:3000/set-password?code={employeeCode}";
            return $@"
            <div style='font-family: Arial, sans-serif; background-color:#f4f4f4; padding:20px;'>
                <div style='max-width:600px; margin:auto; background:#ffffff; border-radius:8px; overflow:hidden;'>

                    <div style='background-color:#1976d2; color:white; padding:20px; text-align:center; font-size:22px; font-weight:bold;'>
                        Welcome to Our Organization
                    </div>

                    <div style='padding:30px; color:#333; font-size:16px;'>

                        <p>Dear <b>{name}</b>,</p>

                        <p>Your employee profile has been created successfully.</p>

                        <p><b>Employee Code:</b> {employeeCode}</p>

                        <p>You can set your password and log in using the following link:</p>

                        <div style='text-align:center; margin:25px 0;'>
                            <a href='{setupLink}' 
                               style='background-color:#1976d2; color:white; padding:12px 25px; 
                                      text-decoration:none; border-radius:5px; display:inline-block;'>
                                Click to Login
                            </a>
                        </div>

                        <p>If you did not request this account, please ignore this email.</p>

                        <p>Best regards,<br/>
                        <b>Kalanjiyam Tech</b></p>

                    </div>

                    <div style='background:#f1f1f1; text-align:center; padding:15px; font-size:12px; color:#666;'>
                        © 2025 Our Organization. All rights reserved.
                    </div>

                </div>
            </div>";
        }
    }
}
