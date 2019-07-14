using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OdeToFood.Data;

namespace OdeToFood {
    public class Program {
        public static void Main(string[] args) {
            //CreateWebHostBuilder(args).Build().Run();
            IWebHost host = CreateWebHostBuilder(args).Build();
            MigrateDatabase(host);
            host.Run();
        }

        private static void MigrateDatabase(IWebHost host) {
            using (IServiceScope scope = host.Services.CreateScope()) {
                OdeToFoodDbContext db = scope.ServiceProvider.GetRequiredService<OdeToFoodDbContext>();
                db.Database.Migrate();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }
}

// Publishing instructions will be here, -o = folder
// dotnet publish -o d:\temp\odetofood (Powershell Windows slashes)
// pushd d:\temp\odetofood
// dotnet run will not work
// dotnet OdeToFood.dll will normally start the csproj startup file unless you have a missing node_modules folder
// Fo our npm issue, open OdeToFood.csproj  XML file and automate the node_modules in any build/publish, see if you can find it
// By extrapolation, I now know that this csproj file is where artifacts can be leftover from exclusions/deletions that I experienced in Xamarin

// pushd and popd - like a stack for remembering which folder you were in

// trying a self contained publish, you need to specify the runtime ID for e.g. Windows 64 bit
// https://docs.microsoft.com/en-us/dotnet/core/rid-catalog
// dotnet publish -o d:\temp\odetofood --self-contained -r win10-x64
// Try running OdeToFood.exe in lieu of dotnet Odetofood.dll?

// IIS hosting (other options include nginX and Apache for linux)

// Database migrations in the OdeToFood.Data folder
// dotnet ef migrations script -s ..\OdeToFood\OdeToFood.csproj


