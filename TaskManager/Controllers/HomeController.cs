using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
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
    public class ReceivingStatus // Class for Valid Receiving Data to Change Status
    {
        public int TaskID { get; set; }
        public string Status { get; set; }
    }

    public class ReceivingData // Class for Valid Receiving Data for any other Queries
    {
        public int TaskID { get; set; } 
        [Required]
        public string Name { get; set; } 
        [Required]
        public string Description { get; set; } 
        [Required]
        public string Executors { get; set; }
        [Required]
        public DateTime PlannedEndDate{ get; set; } // The Date when work planned to be ended
        public int ParentTaskID { get; set; } // (Optional) The ID of the Task of which this task is a part
    }
    public class HomeController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); //The logger, which log all info about exceptions
        private readonly IStringLocalizer<HomeController> _localizer; //Takes data from resources to localize
        private TaskDbContext _db; //Context of our Database
        private readonly ITasks _tasks; //Interface, linked with repos of our model
        public HomeController(ITasks tasks, TaskDbContext db, IStringLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
            _tasks = tasks;
            _db = db;
        }
        [HttpGet] // Секция для обработки GET Запросов
        public IEnumerable<Tasks> GetMajorTasks() => _tasks.GetMajorTasks(); //Receiving Major Tasks (the tasks which aren't child for any other tasks)
        public IEnumerable<Tasks> GetNotCompletedTasks() => _tasks.GetNotCompletedTasks(); // Take all not completed Tasks for our select (to create new task)
        public IActionResult Index() // Building a view with localization
        {
            ViewData["Title"] = _localizer["App.Title"];
            ViewData["Confirmation.No"] = _localizer["Confirmation.No"];
            ViewData["Confirmation.Yes"] = _localizer["Confirmation.Yes"];
            ViewData["Confirmation.Title"] = _localizer["Confirmation.Title"];
            ViewData["Controls.Tip.Create"] = _localizer["Controls.Tip.Create"];
            ViewData["Controls.Tip.Edit"] = _localizer["Controls.Tip.Edit"];
            ViewData["Controls.Tip.Delete"] = _localizer["Controls.Tip.Delete"];
            ViewData["Form.Button.Create"] = _localizer["Form.Button.Create"];
            ViewData["Form.Description"] = _localizer["Form.Description"];
            ViewData["Form.Description.Invalid"] = _localizer["Form.Description.Invalid"];
            ViewData["Form.Description.Placeholder"] = _localizer["Form.Description.Placeholder"];
            ViewData["Form.EndDate"] = _localizer["Form.EndDate"];
            ViewData["Form.EndDate.Invalid"] = _localizer["Form.EndDate.Invalid"];
            ViewData["Form.Executors"] = _localizer["Form.Executors"];
            ViewData["Form.Executors.Invalid"] = _localizer["Form.Executors.Invalid"];
            ViewData["Form.Executors.Placeholder"] = _localizer["Form.Executors.Placeholder"];
            ViewData["Form.Header"] = _localizer["Form.Header"];
            ViewData["Form.Name"] = _localizer["Form.Name"];
            ViewData["Form.Name.Invalid"] = _localizer["Form.Name.Invalid"];
            ViewData["Form.Name.Placeholder"] = _localizer["Form.Name.Placeholder"];
            ViewData["Form.ParentID"] = _localizer["Form.ParentID"];
            ViewData["Form.ParentID.Optional"] = _localizer["Form.ParentID.Optional"];
            ViewData["Sidebar.Title"] = _localizer["Sidebar.Title"];
            ViewData["TaskInfo.Description"] = _localizer["TaskInfo.Description"];
            ViewData["TaskInfo.Executors"] = _localizer["TaskInfo.Executors"];
            ViewData["TaskInfo.Status"] = _localizer["TaskInfo.Status"];
            ViewData["TaskInfo.Time"] = _localizer["TaskInfo.Time"];
            ViewData["TaskInfo.Button.Left"] = _localizer["TaskInfo.Button.Left"];
            ViewData["TaskInfo.Button.Left.Title"] = _localizer["TaskInfo.Button.Left.Title"];
            ViewData["TaskInfo.Button.Right"] = _localizer["TaskInfo.Button.Right"];
            ViewData["TaskInfo.Button.Right.Title"] = _localizer["TaskInfo.Button.Right.Title"];
            @ViewData["Form.Button.Edit"] = _localizer["Form.Button.Edit"];
            HomeIndexViewModel obj = new HomeIndexViewModel();
            obj.GetNotCompletedTasks = _tasks.GetNotCompletedTasks();
            return View(obj);
        } 
        [HttpPost] // Section of POST Processing Methods
        public Tasks GetTaskInfo([FromBody] int taskID) => _tasks.GetTaskInfo(taskID); // Get All Info about selected Task

        public IEnumerable<Tasks> GetSubtasks([FromBody] int taskID) => _tasks.GetSubtasks(taskID); // Get All Subtasks of current task

        public void AddTask([FromBody] ReceivingData data) //Adding a task
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
        public void EditTask([FromBody] ReceivingData data) //Editing a task
        {
            Tasks task = _db.Tasks.Where(p => p.TaskID == data.TaskID).FirstOrDefault();
            task.Name = data.Name;
            task.Description = data.Description;
            task.Executors = data.Executors;
            task.ParentTaskID = data.ParentTaskID;
            _db.SaveChanges();

        }
        public void DeleteTask([FromBody] int taskID) // Deleting a task
        {
            IEnumerable<Tasks> tasks = _db.Tasks.Where(p => p.TaskID == taskID || p.ParentTaskID==taskID).ToList();
            foreach (var task in tasks) _db.Tasks.Remove(task);
            _db.SaveChanges();
        }
        public void ChangeStatus([FromBody] ReceivingStatus data) //Change status with buttons
        {
            Tasks task = _db.Tasks.Where(p => p.TaskID == data.TaskID).FirstOrDefault();
            task.Status = data.Status;
            _db.SaveChanges();
        }
        protected void OnException(ExceptionContext filterContext) //Logging an exception
        {
            logger.Error(filterContext.Exception);
            filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            filterContext.ExceptionHandled = true;
        }
    }
}
