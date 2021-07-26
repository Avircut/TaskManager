using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DAL.EF;
using TaskManager.DAL.Interfaces;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly TaskDbContext _db;
        private readonly ITasks _tasks;
        public HomeController()
        {
            //_tasks = tasks;
            //_db = db;
        }
        public IActionResult Index()
        {
            ViewBag.Title = "Task Manager";
            HomeIndexViewModel obj = new HomeIndexViewModel();
            return View(obj);
        }
    }
}
