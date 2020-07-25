using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListCore.Models
{
    public class EmployeeTaskVM
    {
        // Zadanie
        [Required]
        public int ID { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić datę rozpoczęcia zadania.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić godzinę rozpoczęcia zadania.")]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić datę zakończenia zadania.")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić godzinę zakończenia zadania.")]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić tytuł zadania.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić opis zadania.")]
        public string Description { get; set; }
        public bool IsEnd { get; set; }

        //Employee
        [Required]
        public int EmployeeID { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić imię pracownika.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić nazwisko pracownika.")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić datę urodzenia pracownika.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{dd-MM-yyyy}")]
        public DateTime DayOfBirthday { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić adres e-mail pracownika.")]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić numer telefonu pracownika.")]
        [RegularExpression("^(\\+[0 - 9]{11})$")]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
