using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DAL.Models;

namespace TaskManager.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Tasks> GetMajorTasks { get; set; }
        public IEnumerable<Tasks> GetSubtasks { get; set; }
        public Tasks GetTaskInfo { get; set; }
    }
}
