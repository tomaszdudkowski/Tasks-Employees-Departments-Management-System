using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListCore.Models
{
    public class EmpInTask
    {
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }

        public int ZadanieID { get; set; }
        public Zadanie Zadanie { get; set; }
    }
}
