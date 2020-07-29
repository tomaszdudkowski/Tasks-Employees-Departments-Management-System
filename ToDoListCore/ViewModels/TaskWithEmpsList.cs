using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ToDoListCore.Models;

namespace ToDoListCore.ViewModels
{
    public class TaskWithEmpsList
    {
        //Zadanie
        [Required]
        public int ID { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić datę rozpoczęcia zadania.")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić godzinę rozpoczęcia zadania.")]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić datę zakończenia zadania.")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić godzinę zakończenia zadania.")]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić tytuł zadania.")]
        [StringLength(250)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić opis zadania.")]
        [StringLength(250)]
        public string Description { get; set; }
        public bool IsEnd { get; set; }

        // Lista pracowników przypisanych do zadania
        public List<Employee> EmployeesList { get; set; }
    }
}
