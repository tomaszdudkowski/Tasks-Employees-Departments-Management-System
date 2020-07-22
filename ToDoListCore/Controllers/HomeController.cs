using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoListCore.DAL;
using ToDoListCore.Models;

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
            var tasks = _context.Zadania.Include(z => z.Employees).ThenInclude(e => e.Employee);
            EmployeeTaskVM vM;
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
            }

            /*if(!String.IsNullOrEmpty(SearchString))
            {
                tasks = tasks.Where(t => t.Title.ToLower().Contains(SearchString.ToLower())).ToList();
            }*/
            return View(EmployeeTaskVMList);
        }

        [HttpGet]
        public ViewResult AddTask()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddTask([Bind("ID, StartTime, EndTime, Title, Description, IsEnd")] Zadanie task)
        {
            task.IsEnd = false;
            _context.Zadania.Add(task);
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
            Zadanie task = _context.Zadania.Find(id);
            return View(task);
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
                    _context.Update(task);
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
            Employee empl = new Employee();
            empl.Name = "Tomasz";
            empl.Surname = "Dudkowski";
            empl.EmailAddress = "tomaszszd@gmail.com";
            empl.PhoneNumber = "+48601775210";
            empl.DayOfBirthday = DateTime.Now;

            Department dept = new Department();
            dept.Name = "IT";

            empl.Department = dept;

            Zadanie zadanie = new Zadanie();
            zadanie.Title = "Program ASP.NET Core";
            zadanie.Description = "ABCD";
            zadanie.StartTime = DateTime.Now;
            zadanie.EndTime = DateTime.Now;
            zadanie.IsEnd = false;

            EmpInTask eit = new EmpInTask();
            eit.Zadanie = zadanie;
            eit.Employee = empl;
            _context.ZadaniaInTasks.Add(eit);
            _context.SaveChanges();
        }

        private bool TaskExist(int id)
        {
            return _context.Zadania.Any(t => t.ID == id);
        }
    }
}
