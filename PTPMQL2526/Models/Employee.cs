using System.ComponentModel.DataAnnotations.Schema;

namespace PTPMQL2526.Models
{
    [Table("Employees")]
    public class Employee : Person
    {
        public string EmployeeID { get; set; }

        public int Age { get; set; }

    }
}