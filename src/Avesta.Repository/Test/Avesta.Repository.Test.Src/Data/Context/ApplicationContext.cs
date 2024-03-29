﻿using Avesta.Repository.Test.Src.Data.Model;
using Microsoft.EntityFrameworkCore;
using Avesta.Data.Entity.Context;

namespace Avesta.Repository.Test.Src.Data.Context
{
    public class ApplicationDbContext : AvestaDbContext
    {

        public ApplicationDbContext() : base()
        {
        }
      
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Teacher_School> Teacher_Schools { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlite("Data source = app.db");
            base.OnConfiguring(optionsBuilder);
        }


    }
}
