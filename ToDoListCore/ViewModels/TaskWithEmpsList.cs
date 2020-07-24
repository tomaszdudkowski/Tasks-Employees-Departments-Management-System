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
        [DisplayFormat(DataFormatString = "{dd-MM-yyyy hh:mm}")]
        public DateTime StartTime { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić datę zakończenia zadania.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{dd-MM-yyyy hh:mm}")]
        public DateTime EndTime { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić tytuł zadania.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić opis zadania.")]
        public string Description { get; set; }
        public bool IsEnd { get; set; }

        // Lista pracowników przypisanych do zadania
        public List<Employee> EmployeesList { get; set; }
    }
}
