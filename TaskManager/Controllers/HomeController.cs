using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DAL.EF;
using TaskManager.DAL.Interfaces;
using TaskManager.DAL.Models;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    public class ReceivingData
    {
        public int TaskID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Executors { get; set; }
        [Required]
        public DateTime PlannedEndDate{ get; set; }
        public int ParentTaskID { get; set; }
    }
    public class HomeController : Controller
    {
        private TaskDbContext _db;
        private readonly ITasks _tasks;
        public HomeController(ITasks tasks, TaskDbContext db)
        {
            _tasks = tasks;
            _db = db;
        }
        [HttpGet]
        public IEnumerable<Tasks> GetMajorTasks() => _tasks.GetMajorTasks();
        public IEnumerable<Tasks> GetNotCompletedTasks() => _tasks.GetNotCompletedTasks();
        public IActionResult Index()
        {
            ViewBag.Title = "Task Manager";
            HomeIndexViewModel obj = new HomeIndexViewModel();
            obj.GetNotCompletedTasks = _tasks.GetNotCompletedTasks();
            return View(obj);
        }
        [HttpPost]
        public Tasks GetTaskInfo([FromBody] int taskID) => _tasks.GetTaskInfo(taskID);

        public IEnumerable<Tasks> GetSubtasks([FromBody] int taskID) => _tasks.GetSubtasks(taskID);

        public void AddTask([FromBody] ReceivingData data)
        {
            _db.Database.EnsureCreated();
            Tasks task = new Tasks { 
                Name = data.Name, 
                Description = data.Description, 
                PlannedEndDate = data.PlannedEndDate, 
                Executors = data.Executors, 
                Status = "Назначена", 
                ParentTaskID = data.ParentTaskID, 
                RegisterDate = DateTime.Now};
            _db.Tasks.AddRange(task);
            _db.SaveChanges();

        }
        public void EditTask([FromBody] ReceivingData data)
        {
            Tasks task = _db.Tasks.Where(p => p.TaskID == data.TaskID).FirstOrDefault();
            task.Name = data.Name;
            task.Description = data.Description;
            task.Executors = data.Executors;
            task.ParentTaskID = data.ParentTaskID;
            _db.SaveChanges();

        }
        public void DeleteTask([FromBody] int taskID)
        {
            IEnumerable<Tasks> tasks = _db.Tasks.Where(p => p.TaskID == taskID || p.ParentTaskID==taskID).ToList();
            foreach (var task in tasks) _db.Tasks.Remove(task);
            _db.SaveChanges();
        }
    }
}
