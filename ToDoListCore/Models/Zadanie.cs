using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListCore.Models
{
    public class Zadanie
    {
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
    }
}
