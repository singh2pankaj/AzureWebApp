using Microsoft.AspNetCore.Mvc;
using AzureWebApp.Data;
using AzureWebApp.Models;

namespace AzureWebApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Students.ToList());
        }

        public IActionResult Add()
        {
            _context.Students.Add(new Student
            {
                Name = "Anmol",
                Age = 25,
                Course = "MCA"
            });

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}