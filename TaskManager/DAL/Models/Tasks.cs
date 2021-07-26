using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.DAL.Models
{
    public class Tasks // Из-за неоднозначности с System.Threading.Tasks класс назван Tasks, а не Task
    {
        public int TaskID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Executors { get; set; }
        [Required]
        public DateTime RegisterDate { get; set; }
        public string Status { get; set; }
        [Required]
        public DateTime PlannedEndDate { get; set; }
        public DateTime FactEndDate { get; set; }
        public int ParentTaskID { get; set; }
    }
}
