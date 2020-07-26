using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoListCore.DAL;
using ToDoListCore.Models;
using ToDoListCore.ViewModels;

namespace ToDoListCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ViewResult Main(string SearchString)
        {
            var tasks = _context.Zadania;
            /*EmployeeTaskVM vM;
            List<EmployeeTaskVM> EmployeeTaskVMList = new List<EmployeeTaskVM>();
            foreach(var item in tasks)
            {
                foreach(EmpInTask empl in item.Employees)
                {
                    vM = new EmployeeTaskVM();
                    vM.ID = item.ID;
                    vM.EmployeeID = empl.Employee.EmployeeID;
                    vM.StartTime = item.StartTime;
                    vM.EndTime = item.EndTime;
                    vM.Title = item.Title;
                    vM.Name = empl.Employee.Name;
                    vM.Surname = empl.Employee.Surname;
                    vM.IsEnd = item.IsEnd;
                    EmployeeTaskVMList.Add(vM);
                }
            }*/

            // Do wyszukiwania - naprawić
            /*if(!String.IsNullOrEmpty(SearchString))
            {
                tasks = tasks.Where(t => t.Title.ToLower().Contains(SearchString.ToLower())).ToList();
            }*/
            return View(tasks);
        }

        [HttpGet]
        public ViewResult AddTask()
        {
            var empl = _context.Employees.ToList();
            TaskWithEmpsList twel = new TaskWithEmpsList();
            twel.EmployeesList = empl;
            twel.StartDate = DateTime.Now;
            twel.StartTime = DateTime.Now;
            twel.EndDate = DateTime.Now;
            twel.EndTime = DateTime.Now;
            return View(twel);
        }

        [HttpPost]
        public IActionResult AddTask([Bind("ID, StartDate, StartTime, EndDate, EndTime, Title, Description, IsEnd, EmployeesList")] TaskWithEmpsList taskEmp)
        {
            taskEmp.IsEnd = false;
            EmpInTask eit;
            Zadanie task = new Zadanie();
            Employee empl;

            task.ID = taskEmp.ID;
            task.StartTime = taskEmp.StartTime;
            task.StartDate = taskEmp.StartDate;
            task.EndTime = taskEmp.EndTime;
            task.EndDate = taskEmp.EndDate;
            task.Title = taskEmp.Title;
            task.Description = taskEmp.Description;
            task.IsEnd = taskEmp.IsEnd;

            foreach (var emp in taskEmp.EmployeesList.Where(e => e.checkBoxEmp == true))
            {
                empl = new Employee();
                empl.EmployeeID = emp.EmployeeID;
                empl.Name = emp.Name;
                empl.Surname = emp.Surname;
                empl.DayOfBirthday = emp.DayOfBirthday;
                empl.EmailAddress = emp.EmailAddress;
                empl.PhoneNumber = emp.PhoneNumber;
                empl.DeptID = emp.DeptID;

                eit = new EmpInTask();
                eit.Zadanie = task;
                // Nie dodawać pracownika do relacji jako obiekt tylko dodawać jako ID :)
                // Gdy pracownik już istnieje, a nie jest dodawany razem z zadaniem.
                // Wtedy działa...
                //eit.Employee = empl;
                eit.EmployeeID = empl.EmployeeID;
                _context.ZadaniaInTasks.Add(eit);
            }
            _context.SaveChanges();

            return RedirectToAction("Main");
        }

        [HttpGet]
        public IActionResult DeleteTask(int id)
        {
            Zadanie task = _context.Zadania.Find(id);
            _context.Zadania.Remove(task);
            _context.SaveChanges();
            return RedirectToAction("Main");
        }

        [HttpGet]
        public IActionResult DetailTask(int id)
        {
            var task = _context.Zadania.Include(z => z.Employees).ThenInclude(e => e.Employee).Where(z => z.ID == id).FirstOrDefault();
            TaskWithEmpsList twel = new TaskWithEmpsList();
            twel.ID = task.ID;
            twel.StartTime = task.StartTime;
            twel.EndTime = task.EndTime;
            twel.StartDate = task.StartDate;
            twel.EndDate = task.EndDate;
            twel.Title = task.Title;
            twel.Description = task.Description;
            twel.EmployeesList = new List<Employee>();
            foreach (EmpInTask item in task.Employees)
            {
                twel.EmployeesList.Add(item.Employee);
            }
            return View(twel);
        }

        [HttpGet]
        public IActionResult EditTask(int id)
        {
            Zadanie task = _context.Zadania.Find(id);
            List<EmpInTask> eitList = _context.ZadaniaInTasks.Where(f => f.ZadanieID == id).ToList();
            TaskWithEmpsList twel = new TaskWithEmpsList();
            List<Employee> empList = _context.Employees.ToList();

            foreach (var empInTask in eitList)
            {
                foreach (var emp in empList)
                {
                    if(empInTask.EmployeeID == emp.EmployeeID)
                    {
                        emp.checkBoxEmp = true;
                    }
                }
            }

            twel.ID = task.ID;
            twel.StartDate = task.StartDate;
            twel.StartTime = task.StartTime;
            twel.EndDate = task.EndDate;
            twel.EndTime = task.EndTime;
            twel.Title = task.Title;
            twel.Description = task.Description;
            twel.IsEnd = task.IsEnd;
            twel.EmployeesList = empList;

            return View(twel);
        }

        [HttpPost]
        public IActionResult EditTask(int id, [Bind("ID, StartDate, StartTime, EndDate, EndTime, Title, Description, IsEnd, EmployeesList")] TaskWithEmpsList taskEmp)
        {
            taskEmp.IsEnd = false;
            EmpInTask eit;
            Zadanie task = new Zadanie();
            Employee empl;

            task.ID = taskEmp.ID;
            task.StartTime = taskEmp.StartTime;
            task.StartDate = taskEmp.StartDate;
            task.EndTime = taskEmp.EndTime;
            task.EndDate = taskEmp.EndDate;
            task.Title = taskEmp.Title;
            task.Description = taskEmp.Description;
            task.IsEnd = taskEmp.IsEnd;

            foreach (var emp in taskEmp.EmployeesList.Where(e => e.checkBoxEmp == true))
            {
                empl = new Employee();
                empl.EmployeeID = emp.EmployeeID;
                empl.Name = emp.Name;
                empl.Surname = emp.Surname;
                empl.DayOfBirthday = emp.DayOfBirthday;
                empl.EmailAddress = emp.EmailAddress;
                empl.PhoneNumber = emp.PhoneNumber;
                empl.DeptID = emp.DeptID;

                eit = new EmpInTask();
                eit.ZadanieID = task.ID;
                // Nie dodawać pracownika do relacji jako obiekt tylko dodawać jako ID :)
                // Gdy pracownik już istnieje, a nie jest dodawany razem z zadaniem.
                // Wtedy działa...
                //eit.Employee = empl;
                eit.EmployeeID = empl.EmployeeID;
                if(TaskEmp(eit.EmployeeID, eit.ZadanieID) == false)
                {
                    _context.ZadaniaInTasks.Add(eit);
                    _context.SaveChanges();
                }
            }

            foreach (var emp in taskEmp.EmployeesList.Where(e => e.checkBoxEmp == false))
            {
                eit = new EmpInTask();
                eit.ZadanieID = task.ID;
                eit.EmployeeID = emp.EmployeeID;
                if(TaskEmp(eit.EmployeeID, eit.ZadanieID) == true)
                {
                    _context.ZadaniaInTasks.Remove(eit);
                    _context.SaveChanges();
                }
            }

            if (id != task.ID)
            {
                return Content("ID jest nie prawidłowe.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Zadania.Update(task);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExist(task.ID))
                    {
                        return Content("Zadanie nie istnieje");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Main));
            }
            return View(task);
        }

        [HttpGet]
        public IActionResult EndTask(int id)
        {
            Zadanie task = _context.Zadania.Find(id);

            task.IsEnd = true;
            _context.Update(task);
            _context.SaveChanges();
            return RedirectToAction(nameof(Main));
        }

        [HttpGet]
        public IActionResult ResponeTask(int id)
        {
            Zadanie task = _context.Zadania.Find(id);

            task.IsEnd = false;
            _context.Update(task);
            _context.SaveChanges();
            return RedirectToAction(nameof(Main));
        }

        private bool TaskExist(int id)
        {
            return _context.Zadania.Any(t => t.ID == id);
        }

        private bool TaskEmp(int EmplID, int TaskID)
        {
            return _context.ZadaniaInTasks.Any(t => t.EmployeeID == EmplID && t.ZadanieID == TaskID);
        }
    }
}
