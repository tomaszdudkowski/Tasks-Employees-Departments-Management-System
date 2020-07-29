using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ToDoListCore.Models;

namespace ToDoListCore.ViewModels
{
    public class EmployeeDepartment
    {
        //Department
        [Required]
        public int? DepartmentID { get; set; }
        public string DeptName { get; set; }

        //Employee
        [Required]
        public int EmployeeID { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić imię pracownika.")]
        public string EmplName { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić nazwisko pracownika.")]
        public string EmplSurname { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić datę urodzenia pracownika.")]
        public DateTime DayOfBirthday { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić adres e-mail pracownika.")]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić numer telefonu pracownika.")]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
