using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework.Context
{
    public class IMandCRMContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseSqlServer(connectionString: @"Data Source=LAPTOP-Q5KQP3V5\SQLEXPRESS;Initial Catalog=Measurement;persist security info=True;user id=qbot;Password=123;");          
           
        }

        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<ArrayDesign> ArrayDesigns { get; set; }
        public DbSet<ArrayMeasurement> ArrayMeasurements { get; set; }

    }
}
