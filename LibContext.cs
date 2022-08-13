using kursADO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursADO
{
    public class LibContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public LibContext() : base("FullHospitalDB")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Disease>().HasRequired<Category>(s => s.Category)
            .WithMany(g => g.Diseases)
            .HasForeignKey(s => s.CategoryId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Doctor>().HasRequired<Category>(s => s.Category)
            .WithMany(g => g.Doctors)
            .HasForeignKey(s => s.CategoryId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Category>().HasRequired<Department>(s => s.Department)
            .WithMany(g => g.Categories)
            .HasForeignKey(s => s.DepartmentId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Room>().HasRequired<Department>(s => s.Department)
            .WithMany(g => g.Rooms)
            .HasForeignKey(s => s.DepartmentId).WillCascadeOnDelete(false);
            modelBuilder.Entity<Patient>().HasRequired<Room>(s => s.Room)
            .WithMany(g => g.Patients)
            .HasForeignKey(s => s.RoomId).WillCascadeOnDelete(false);
        }
    }
}

