using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Data.Models
{
    public class Tasks // Из-за неоднозначности с System.Threading.Tasks класс назван Tasks, а не Task
    {
        public int TaskID;
        public string Name;
        public string Description;
        public string Executors;
        public DateTime RegisterDate;
        public string Status;
        public DateTime PlannedLabor;
        public DateTime FactLabor;
        public DateTime CompletedDate;
        public int ParentTaskID;
    }
}
