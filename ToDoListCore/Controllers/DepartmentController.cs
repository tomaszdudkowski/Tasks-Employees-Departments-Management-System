﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    }
}
