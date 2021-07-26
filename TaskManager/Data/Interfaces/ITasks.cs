using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Data.Models;

namespace TaskManager.Data.Interfaces
{
    interface ITasks
    {
        IEnumerable<Tasks> GetTasks();
    }
}
