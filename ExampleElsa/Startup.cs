using Elsa.Activities.Email.Extensions;
using Elsa.Activities.Http.Extensions;
using Elsa.Activities.Timers.Extensions;
using Elsa.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Elsa.Persistence.EntityFrameworkCore.Extensions;
using Elsa.Persistence.EntityFrameworkCore.DbContexts;
using Elsa.Dashboard.Extensions;
using ExampleElsa.Handlers;
using ExampleElsa.Services;
using ExampleElsa.Services.Imp;
using Fluid;
namespace ExampleElsa
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            //services.AddControllersWithViews();
            services
                .AddElsa(elsa=> elsa.AddEntityFrameworkStores<SqlServerContext>(options=>options.UseSqlServer(Configuration.GetConnectionString("SqlDB"))))
                .AddHttpActivities(options => options.Bind(Configuration.GetSection("Elsa:Http")))
                .AddEmailActivities(options => options.Bind(Configuration.GetSection("Elsa:Smtp")))
                .AddTimerActivities(options => options.Bind(Configuration.GetSection("Elsa:Timers")));
            
            // Add our PasswordHasher service.
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            // Add services used for the workflows dashboard.
            services.AddElsaDashboard();
                // Add our liquid handler.
            services.AddNotificationHandlers(typeof(LiquidConfigurationHandler));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();

            // Add Elsa's middleware to handle HTTP requests to workflows.  
            app.UseHttpActivities();

            app.UseRouting();

            app.UseEndpoints(
                endpoints =>
                {
                    // Blazor stuff.
                    endpoints.MapBlazorHub();
                    endpoints.MapFallbackToPage("/_Host");

                    // Attribute-based routing stuff.
                    endpoints.MapControllers();
                });
        }
    }
}
