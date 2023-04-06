using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskAPI.Model
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpId { get; set; }
        public int EmpCode { get; set; }
        public string ? EmpName { get; set; }
        public int DeptId { get; set; }
        public int Salary { get; set; }

        [ForeignKey("DeptId")]
        public virtual Department DeptName { get; set; }
    }
}
