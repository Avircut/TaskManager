using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.DAL.Models
{
    public class Tasks // Из-за неоднозначности с System.Threading.Tasks класс назван Tasks, а не Task
    {
        public int TaskID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Executors { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Status { get; set; }
        public DateTime PlannedLabor { get; set; }
        public DateTime FactLabor { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime FactEndDate { get; set; }
        public int ParentTaskID { get; set; }
    }
}
