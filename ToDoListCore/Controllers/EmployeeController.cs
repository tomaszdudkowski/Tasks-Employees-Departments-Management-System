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
                if(item.Department == null)
                {
                    ed.DeptName = "";
                } else
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
            List<Department> departments = new List<Department>();
            departments = _context.Departments.ToList();
            List<EmployeeDepartment> departmentsEDVM = new List<EmployeeDepartment>();
            EmployeeDepartment ed = new EmployeeDepartment();
            foreach (var item in departments)
            {
                ed = new EmployeeDepartment();
                ed.DepartmentID = item.DepartmentID;
                ed.DeptName = item.Name;
                departmentsEDVM.Add(ed);
            }
            ViewBag.ListOfDepartments = departmentsEDVM;
            return View(ed);
        }

        [HttpPost]
        public IActionResult AddEmployee([Bind("EmployeeID, EmplName, EmplSurname, DayOfBirthday, EmailAddress, PhoneNumber, DepartmentID")] EmployeeDepartment employeeDep)
        {
            Employee employee = new Employee();
            employee.EmployeeID = employeeDep.EmployeeID;
            employee.DeptID = employeeDep.DepartmentID;
            employee.Name = employeeDep.EmplName;
            employee.Surname = employeeDep.EmplSurname;
            employee.DayOfBirthday = employeeDep.DayOfBirthday;
            employee.EmailAddress = employeeDep.EmailAddress;
            employee.PhoneNumber = employeeDep.PhoneNumber;
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
            if(emp.Department == null)
            {
                ewzl.DeptName = "";
            } else
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
            List<Department> departments = new List<Department>();
            departments = _context.Departments.ToList();
            List<EmployeeDepartment> departmentsEDVM = new List<EmployeeDepartment>();
            EmployeeDepartment ed1 = new EmployeeDepartment();

            ed1 = new EmployeeDepartment();
            ed1.EmployeeID = emp.EmployeeID;
            ed1.EmplName = emp.Name;
            ed1.EmplSurname = emp.Surname;
            ed1.EmailAddress = emp.EmailAddress;
            ed1.PhoneNumber = emp.PhoneNumber;
            ed1.DayOfBirthday = emp.DayOfBirthday;
            ed1.DepartmentID = emp.DeptID;
            if(emp.Department == null)
            {
                ed1.DeptName = "";
            } else
            {
                ed1.DeptName = emp.Department.Name;
            }
            departmentsEDVM.Add(ed1);

            foreach (var item in departments)
            {
                if(item.DepartmentID != emp.DeptID)
                {
                    ed1 = new EmployeeDepartment();
                    ed1.DepartmentID = item.DepartmentID;
                    ed1.DeptName = item.Name;
                    departmentsEDVM.Add(ed1);
                }
            }

            ViewBag.ListOfDepartments = departmentsEDVM;

            return View(departmentsEDVM[0]);
        }

        [HttpPost]
        public IActionResult EditEmployee(int id, EmployeeDepartment employeeDep)
        {
            Employee employee = new Employee();
            employee.EmployeeID = employeeDep.EmployeeID;
            employee.Name = employeeDep.EmplName;
            employee.Surname = employeeDep.EmplSurname;
            employee.DayOfBirthday = employeeDep.DayOfBirthday;
            employee.EmailAddress = employeeDep.EmailAddress;
            employee.PhoneNumber = employeeDep.PhoneNumber;
            employee.DeptID = employeeDep.DepartmentID;


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
