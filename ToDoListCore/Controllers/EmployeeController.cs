using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoListCore.DAL;
using ToDoListCore.Models;
using ToDoListCore.ViewModels;

namespace ToDoListCore.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly ILogger<EmployeeController> _logger;

        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context, ILogger<EmployeeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Main()
        {
            var employees = _context.Employees.Include(e => e.Department);
            List<EmployeeDepartment> employeeDepartmentsLists = new List<EmployeeDepartment>();
            EmployeeDepartment ed;
            foreach (var item in employees)
            {
                ed = new EmployeeDepartment();
                ed.EmployeeID = item.EmployeeID;
                ed.EmplName = item.Name;
                ed.EmplSurname = item.Surname;
                ed.EmailAddress = item.EmailAddress;
                ed.PhoneNumber = item.PhoneNumber;
                ed.DeptName = item.Department.Name;

                employeeDepartmentsLists.Add(ed);
            }

            return View(employeeDepartmentsLists);
        }

        [HttpGet]
        public ViewResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddEmployee([Bind("EmployeeID, Name, Surname, DayOfBirthday, EmailAddress, PhoneNumber")] Employee employee)
        {
            Department dept = new Department();
            dept.Name = "IT";
            employee.Department = dept;
            _context.Add(employee);
            _context.SaveChanges();
            return RedirectToAction("Main");
        }

        [HttpGet]
        public IActionResult DetailEmployee(int id)
        {
            var emp = _context.Employees.Include(e => e.Department).Include(e => e.Zadania).ThenInclude(z => z.Zadanie).Where(e => e.EmployeeID == id).FirstOrDefault();
            EmployeeWithZadaniaList ewzl = new EmployeeWithZadaniaList();
            ewzl.EmployeeID = emp.EmployeeID;
            ewzl.Name = emp.Name;
            ewzl.Surname = emp.Surname;
            ewzl.EmailAddress = emp.EmailAddress;
            ewzl.PhoneNumber = emp.PhoneNumber;
            ewzl.DayOfBirthday = emp.DayOfBirthday;
            ewzl.DeptName = emp.Department.Name;
            ewzl.ZadaniaList = new List<Zadanie>();
            foreach (EmpInTask item in emp.Zadania)
            {
                ewzl.ZadaniaList.Add(item.Zadanie);
            }
            return View(ewzl);
        }

        [HttpGet]
        public IActionResult EditEmployee(int id)
        {
            Employee emp = _context.Employees.Find(id);
            return View(emp);
        }

        [HttpPost]
        public IActionResult EditEmployee(int id, [Bind("EmployeeID, Name, Surname, DayOfBirthday, EmailAddress, PhoneNumber, DeptID, checkBoxEmp, Zadania, Department")] Employee employee)
        {
            if (id != employee.EmployeeID)
            {
                return Content("ID jest nie prawidłowe.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Employees.Update(employee);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExist(id))
                    {
                        return Content("Pracownik nie istnieje");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Main));
            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult DeleteEmployee(int id)
        {
            Employee empl = _context.Employees.Find(id);
            _context.Employees.Remove(empl);
            _context.SaveChanges();
            return RedirectToAction("Main");
        }

        private bool EmployeeExist(int id)
        {
            return _context.Employees.Any(e => e.EmployeeID == id);
        }
    }
}
