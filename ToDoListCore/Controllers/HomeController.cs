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
            var tasks = _context.Zadania.ToList();
            if(!String.IsNullOrEmpty(SearchString))
            {
                tasks = tasks.Where(t => t.Title.ToLower().Contains(SearchString.ToLower())).ToList();
            }
            return View(tasks);
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

        private bool TaskExist(int id)
        {
            return _context.Zadania.Any(t => t.ID == id);
        }
    }
}
