using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DAL.Models;

namespace TaskManager.DAL.Interfaces
{
    public interface ITasks
    {
        IEnumerable<Tasks> GetMajorTasks();
        Tasks GetTaskInfo(int taskID);
        IEnumerable<Tasks> GetSubtasks(int taskID);
    }
}
