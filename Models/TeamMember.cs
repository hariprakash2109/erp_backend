using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("team_members")]
public class TeamMember
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("employee_code")]
    public string? EmployeeCode { get; set; }

    [Column("first_name")]
    public string? FirstName { get; set; }

    [Column("last_name")]
    public string? LastName { get; set; }

    [Column("gender")]
    public string? Gender { get; set; }

    [Column("date_of_birth")]
    public DateTime? DateOfBirth { get; set; }

    [Column("date_of_joining")]
    public DateTime? DateOfJoining { get; set; }

    [Column("department")]
    public string? Department { get; set; }

    [Column("designation")]
    public string? Designation { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("status")]
    public string? Status { get; set; }

    [Column("official_email")]
    public string? OfficialEmail { get; set; }

    [Column("phone_number")]
    public string? PhoneNumber { get; set; }
    [Column("password")]
    public string? Password { get; set; }
}
