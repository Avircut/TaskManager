using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskManager.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Executors = table.Column<string>(nullable: false),
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    PlannedEndDate = table.Column<DateTime>(nullable: false),
                    FactEndDate = table.Column<DateTime>(nullable: false),
                    ParentTaskID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskID);
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "TaskID", "Description", "Executors", "FactEndDate", "Name", "ParentTaskID", "PlannedEndDate", "RegisterDate", "Status" },
                values: new object[,]
                {
                    { 1, "Создать проект в Visual Studio на ASP.NET Core.", "Вячеслав Ломов", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Создать приложение \"Планировщик задач\"", 0, new DateTime(2021, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Выполняется" },
                    { 2, "Провести анализ технического задания и определить потенциальные методы решения задач", "Вячеслав Ломов, Google", new DateTime(2021, 7, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Продумать структуру проекта", 1, new DateTime(2021, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Выполнено" },
                    { 3, "Определиться с паттерном проектирования. Структурно разделить проект по слоям.", "Вячеслав Ломов", new DateTime(2021, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Реализовать каркас структуры", 1, new DateTime(2021, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 7, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Выполнено" },
                    { 4, "Реализовать базовые модели, интерфейсы, репозитории и контекст данных", "Вячеслав Ломов", new DateTime(2021, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Реализовать DAL", 1, new DateTime(2021, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Выполнено" },
                    { 5, "По итогам тестового задания получить приглашение на собеседование и пройти его", "Вячеслав Ломов", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Пройти собеседование", 0, new DateTime(2021, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Назначена" },
                    { 6, "По итогам собеседования получить приглашение на прохождение стажировки", "Вячеслав Ломов", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Пройти на стажировку и стать джуном", 0, new DateTime(2021, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Назначена" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
