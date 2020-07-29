using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListCore.Models
{
    public class Zadanie
    {
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

        //Navigational Property
        public virtual ICollection<EmpInTask> Employees { get; set; }
    }
}
