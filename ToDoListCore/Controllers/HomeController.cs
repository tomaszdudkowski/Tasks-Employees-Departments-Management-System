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
            return View(twel);
        }

        [HttpPost]
        public IActionResult AddTask([Bind("ID, StartTime, EndTime, Title, Description, IsEnd, EmployeesList")] TaskWithEmpsList taskEmp)
        {
            taskEmp.IsEnd = false;
            EmpInTask eit;
            Zadanie task = new Zadanie();
            Employee empl;

            task.ID = taskEmp.ID;
            task.StartTime = taskEmp.StartTime;
            task.EndTime = taskEmp.EndTime;
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
            return View(task);
        }

        [HttpPost]
        public IActionResult EditTask(int id, [Bind("ID, StartTime, EndTime, Title, Description, IsEnd")] Zadanie task)
        {
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

        public void DodajTemp()
        {
            Employee empl1 = new Employee();
            empl1.Name = "Tomasz";
            empl1.Surname = "Dudkowski";
            empl1.EmailAddress = "tomaszszd@gmail.com";
            empl1.PhoneNumber = "+48601775210";
            empl1.DayOfBirthday = DateTime.Now;

            Employee empl2 = new Employee();
            empl2.Name = "Jan";
            empl2.Surname = "Kowalski";
            empl2.EmailAddress = "jan_kowalski@gmail.com";
            empl2.PhoneNumber = "+48734847319";
            empl2.DayOfBirthday = DateTime.Now;

            Department dept = new Department();
            dept.Name = "IT";

            empl1.Department = dept;
            empl2.Department = dept;

            Zadanie zadanie = new Zadanie();
            zadanie.Title = "Program ASP.NET Core";
            zadanie.Description = "ABCD";
            zadanie.StartTime = DateTime.Now;
            zadanie.EndTime = DateTime.Now;
            zadanie.IsEnd = false;

            EmpInTask eit = new EmpInTask();
            eit.Zadanie = zadanie;
            eit.Employee = empl1;

            EmpInTask eit2 = new EmpInTask();
            eit2.Zadanie = zadanie;
            eit2.Employee = empl2;

            _context.ZadaniaInTasks.Add(eit);
            _context.ZadaniaInTasks.Add(eit2);
            _context.SaveChanges();
        }

        private bool TaskExist(int id)
        {
            return _context.Zadania.Any(t => t.ID == id);
        }
    }
}
