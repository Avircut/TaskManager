using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DAL.EF;
using TaskManager.DAL.Interfaces;
using TaskManager.DAL.Models;

namespace TaskManager.DAL.Repository
{
    public class TasksRepository :ITasks
    {
        private readonly TaskDbContext _db;
        public TasksRepository(TaskDbContext db)
        {
            db = _db;
        }
        public IEnumerable<Tasks> GetMajorTasks() => _db.Tasks.ToList();
        public IEnumerable<Tasks> GetSubtasks(int taskID) => _db.Tasks.Where(p => p.ParentTaskID == taskID).ToList();
        public Tasks GetTaskInfo(int taskID) => _db.Tasks.FirstOrDefault(p => p.TaskID == taskID);
    }
}
