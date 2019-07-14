using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OdeToFood.Data;

namespace OdeToFood {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }
     
        public IConfiguration Configuration { get; } // To access appsettings.json

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContextPool<OdeToFoodDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("OdeToFoodDb"));
            });
            services.AddScoped<IRestaurantData, SqlRestaurantData>(); // Hello SQL server
            //services.AddSingleton<IRestaurantData, InMemoryRestaurantData>(); // Goodbye in-memory "database"

            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Middleware, process HTTP messages, order matters            
            app.UseHttpsRedirection();
            app.UseStaticFiles(); // Allows static files from wwwroot (default on), when used without arguments.
            app.UseNodeModules(env); // Allows serving of static files from node_modules folder, requires library nuget
            app.UseCookiePolicy();
            //app.UseGoogleAuthentication();

            app.UseMvc();
            app.Use(SayHelloMiddleware);
        }

        // Custom middleware, might be good for a bad request, wrong turn or something
        private RequestDelegate SayHelloMiddleware(RequestDelegate arg) {
            // Http context delegate, try https://localhost:44307/hello
            return async ctx => {
                if (ctx.Request.Path.StartsWithSegments("/hello")) {
                    await ctx.Response.WriteAsync("Hello World!");
                } else {
                    //await next(ctx);
                }               
            };
        }
    }
}
