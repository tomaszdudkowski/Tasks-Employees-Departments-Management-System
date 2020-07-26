using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListCore.Models
{
    public class Department
    {
        [Required]
        public int DepartmentID { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić nazwę działu.")]
        public string Name { get; set; }
        public string Description { get; set; }

        //Navigational Property
        public ICollection<Employee> Employees { get; set; }
        
    }
}
