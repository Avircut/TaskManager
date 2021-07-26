using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DAL.Models;

namespace TaskManager.DAL.EF
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { Database.EnsureCreated(); }

        public DbSet<Tasks> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tasks>()
                .HasKey(t => t.TaskID);
            modelBuilder.Entity<Tasks>()
                .HasData(
                    new Tasks {
                        TaskID=1,
                        Name = "Создать приложение \"Планировщик задач\"",
                        Description = "Создать проект в Visual Studio на ASP.NET Core.",
                        Executors = "Вячеслав Ломов",
                        RegisterDate = new DateTime(2021, 07, 22),
                        PlannedEndDate = new DateTime(2021, 07, 29),
                        Status = "Выполняется" },
                    new Tasks{
                        TaskID=2,
                        Name="Продумать структуру проекта",
                        Description = "Провести анализ технического задания и определить потенциальные методы решения задач",
                        Executors = "Вячеслав Ломов, Google",
                        RegisterDate = new DateTime(2021,07,22),
                        PlannedEndDate = new DateTime(2021,07,23),
                        Status = "Выполнено",
                        FactEndDate = new DateTime(2021,07,24),
                        ParentTaskID=1},
                    new Tasks{
                        TaskID=3,
                        Name="Реализовать каркас структуры",
                        Description="Определиться с паттерном проектирования. Структурно разделить проект по слоям.",
                        Executors = "Вячеслав Ломов",
                        RegisterDate = new DateTime(2021,07,24),
                        PlannedEndDate = new DateTime(2021,07,25),
                        Status = "Выполнено",
                        FactEndDate = new DateTime(2021, 07, 25),
                        ParentTaskID=1},
                    new Tasks{
                        TaskID=4,
                        Name="Реализовать DAL",
                        Description = "Реализовать базовые модели, интерфейсы, репозитории и контекст данных",
                        Executors = "Вячеслав Ломов",
                        RegisterDate = new DateTime(2021,07,25),
                        PlannedEndDate = new DateTime(2021,07,25),
                        Status="Выполнено",
                        FactEndDate= new DateTime(2021,07,25),
                        ParentTaskID=1},
                    new Tasks{
                        TaskID=5,
                        Name = "Пройти собеседование",
                        Description = "По итогам тестового задания получить приглашение на собеседование и пройти его",
                        Executors = "Вячеслав Ломов",
                        RegisterDate = new DateTime(2021, 07, 26),
                        PlannedEndDate = new DateTime(2021, 08, 01),
                        Status = "Назначена"},
                    new Tasks{
                        TaskID=6,
                        Name = "Пройти на стажировку и стать джуном",
                        Description = "По итогам собеседования получить приглашение на прохождение стажировки",
                        Executors = "Вячеслав Ломов",
                        RegisterDate = new DateTime(2021,07,26),
                        PlannedEndDate = new DateTime(2021,08,10),
                        Status="Назначена"});
        }
        
    }
}
