using System.ComponentModel.DataAnnotations;

namespace erp.Models
{
    public class Employee
    {
        public int id { get; set; }   // S.No (Primary Key)

        [Required]
        public string employee_id { get; set; }

        [Required]
        public string password { get; set; }
    }
}
