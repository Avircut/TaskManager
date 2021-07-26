using System;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DAL;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.DAL.EF
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<TaskDbContext>
    {
        public TaskDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaskDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TaskManager;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new TaskDbContext(optionsBuilder.Options);
        }
    }
}
