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
                if (item.Department == null)
                {
                    ed.DeptName = "";
                }
                else
                {
                    ed.DeptName = item.Department.Name;
                }


                employeeDepartmentsLists.Add(ed);
            }

            return View(employeeDepartmentsLists);
        }

        [HttpGet]
        public ViewResult AddEmployee()
        {
            List<Department> departments = _context.Departments.ToList();

            EmployeeDepartment ed = new EmployeeDepartment();
            ed.departmentsEDVM = new List<Department>();
            foreach (var item in departments)
            {
                ed.departmentsEDVM.Add(item);
            }
            ed.DayOfBirthday = DateTime.Now;
            return View(ed);
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeDepartment employeeDep)
        {
            List<Department> departments = _context.Departments.ToList();
            employeeDep.departmentsEDVM = new List<Department>();
            foreach (var item in departments)
            {
                employeeDep.departmentsEDVM.Add(item);
            }

            if (ModelState.IsValid)
            {
                Employee employee = new Employee();
                employee.DeptID = employeeDep.DepartmentID;
                employee.EmployeeID = employeeDep.EmployeeID;
                employee.Name = employeeDep.EmplName;
                employee.Surname = employeeDep.EmplSurname;
                employee.DayOfBirthday = employeeDep.DayOfBirthday;
                employee.EmailAddress = employeeDep.EmailAddress;
                employee.PhoneNumber = employeeDep.PhoneNumber;
                _context.Add(employee);
                _context.SaveChanges();
                return RedirectToAction("Main");
            }
            return View(employeeDep);

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
            if (emp.Department == null)
            {
                ewzl.DeptName = "";
            }
            else
            {
                ewzl.DeptName = emp.Department.Name;
            }
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
            List<Department> departments = _context.Departments.ToList();
            EmployeeDepartment ed;

            ed = new EmployeeDepartment();
            ed.EmployeeID = emp.EmployeeID;
            ed.EmplName = emp.Name;
            ed.EmplSurname = emp.Surname;
            ed.EmailAddress = emp.EmailAddress;
            ed.PhoneNumber = emp.PhoneNumber;
            ed.DayOfBirthday = emp.DayOfBirthday;
            ed.DepartmentID = emp.DeptID;
            ed.departmentsEDVM = new List<Department>();
            if (emp.Department == null)
            {
                ed.DeptName = "";
            }
            else
            {
                ed.DeptName = emp.Department.Name;
                ed.departmentsEDVM.Add(emp.Department);
            }

            foreach (var item in departments)
            {
                if (item.DepartmentID != emp.DeptID)
                {
                    ed.departmentsEDVM.Add(item);
                }
            }

            return View(ed);
        }

        [HttpPost]
        public IActionResult EditEmployee(int id, EmployeeDepartment employeeDep)
        {
            List<Department> departments = _context.Departments.ToList();

            Employee employee = new Employee();
            employee.EmployeeID = employeeDep.EmployeeID;
            employee.Name = employeeDep.EmplName;
            employee.Surname = employeeDep.EmplSurname;
            employee.DayOfBirthday = employeeDep.DayOfBirthday;
            employee.EmailAddress = employeeDep.EmailAddress;
            employee.PhoneNumber = employeeDep.PhoneNumber;
            employee.DeptID = employeeDep.DepartmentID;
            employeeDep.departmentsEDVM = new List<Department>();

            foreach (var item in departments)
            {
                employeeDep.departmentsEDVM.Add(item);
            }

            if (id != employeeDep.EmployeeID)
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
            return View(employeeDep);
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
