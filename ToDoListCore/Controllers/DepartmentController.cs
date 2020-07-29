using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoListCore.DAL;
using ToDoListCore.Models;

namespace ToDoListCore.Controllers
{
    public class DepartmentController : Controller
    {

        private readonly ILogger<DepartmentController> _logger;

        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context, ILogger<DepartmentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var DepartmentList = _context.Departments;
            return View(DepartmentList);
        }

        [HttpGet]
        public ViewResult AddDepartment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddDepartment([Bind("DepartmentID, Name, Description")] Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DetailDepartment(int id)
        {
            var dep = _context.Departments.Include(e => e.Employees).Where(d => d.DepartmentID == id).FirstOrDefault();
            
            return View(dep);
        }

        [HttpGet]
        public IActionResult DeleteDepartment(int id)
        {
            Department dept = _context.Departments.Find(id);
            var empl = _context.Employees.Where(e => e.DeptID == id);

            foreach (var e in empl)
            {
                e.DeptID = null;
                e.Department.Name = "";
            }

            _context.Departments.Remove(dept);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
