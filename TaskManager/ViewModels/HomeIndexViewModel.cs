using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DAL.Models;

namespace TaskManager.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Tasks> GetTasks { get; set; }
    }
}
