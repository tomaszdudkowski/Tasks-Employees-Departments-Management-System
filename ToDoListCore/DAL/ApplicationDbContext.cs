using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoListCore.Models;

namespace ToDoListCore.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Models.Zadanie> Zadania { get; set; }
        public DbSet<Models.Employee> Employees { get; set; }
        public DbSet<Models.Department> Departments { get; set; }
        public DbSet<Models.EmpInTask> ZadaniaInTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne<Department>(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DeptID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<EmpInTask>()
                .HasKey(e => new { e.EmployeeID, e.ZadanieID });

            modelBuilder.Entity<EmpInTask>()
                .HasOne<Employee>(e => e.Employee)
                .WithMany(t => t.Zadania);

            modelBuilder.Entity<EmpInTask>()
                .HasOne<Zadanie>(t => t.Zadanie)
                .WithMany(e => e.Employees);
        }
    }
}
