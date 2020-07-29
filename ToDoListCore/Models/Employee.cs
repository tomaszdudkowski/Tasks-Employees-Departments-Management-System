using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListCore.Models
{
    public class Employee
    {
        [Required]
        public int EmployeeID { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić imię pracownika.")]
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = "Proszę wprowadzić nazwisko pracownika.")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić datę urodzenia pracownika.")]
        public DateTime DayOfBirthday { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić adres e-mail pracownika.")]
        [EmailAddress]
        [StringLength(250)]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić numer telefonu pracownika.")]
        [Phone]
        public string PhoneNumber { get; set; }

        [NotMapped]
        public bool checkBoxEmp { get; set; }

        //Navigational Property
        public virtual ICollection<EmpInTask> Zadania { get; set; }

        public int? DeptID { get; set; }
        public Department Department { get; set; }
    }
}
