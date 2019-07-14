using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using OdeToFood.Core;

namespace OdeToFood.Data {
    public class OdeToFoodDbContext : DbContext {
        public OdeToFoodDbContext(DbContextOptions <OdeToFoodDbContext> options) : base(options) {

        }

        public DbSet<Restaurant> Restaurants { get; set; } // Set up this Restaurants database table with the model <Restaurant> then do the initial migrations
        // The migration from the command line (not PMC) is: dotet ef migrations add initialcreate 
        // If the csproj is in another folder, add -s ..\OdeToFood\OdeToFood.csproj
        // Then update the DB by: dotnet ef database update -s <path to csproj as above>

    }
}
