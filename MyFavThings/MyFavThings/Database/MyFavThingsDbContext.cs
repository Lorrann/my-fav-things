using Microsoft.EntityFrameworkCore;
using MyFavThings.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFavThings.Database
{
    public class MyFavThingsDbContext : DbContext
    {
        public MyFavThingsDbContext(DbContextOptions<MyFavThingsDbContext> options) 
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
