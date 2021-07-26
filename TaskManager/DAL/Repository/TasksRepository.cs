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
        public IEnumerable<Tasks> GetTasks()
        {
            List<Tasks> tasksList = new List<Tasks>();
            return tasksList;
        }
    }
}
