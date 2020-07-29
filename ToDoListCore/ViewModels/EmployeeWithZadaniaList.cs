using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ToDoListCore.Models;

namespace ToDoListCore.ViewModels
{
    public class EmployeeWithZadaniaList
    {
        // Pracownik
        [Required]
        public int EmployeeID { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić imię pracownika.")]
        [StringLength(250)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić nazwisko pracownika.")]
        [StringLength(250)]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić datę urodzenia pracownika.")]
        public DateTime DayOfBirthday { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić adres e-mail pracownika.")]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić numer telefonu pracownika.")]
        [Phone]
        public string PhoneNumber { get; set; }

        //Dział pracownika
        [Required]
        public int DepartmentID { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić nazwę działu.")]
        [StringLength(250)]
        public string DeptName { get; set; }

        //Lista zadań przypisana do pracownika
        public List<Zadanie> ZadaniaList { get; set; }
    }
}
