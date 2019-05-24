using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BinaryParkingClientServerNiverovskyi.Models.Transaction;
using BinaryParkingClientServerNiverovskyi.Models.Vehicle;


namespace BinaryParkingClientServerNiverovskyi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the 
        //container.
        public void ConfigureServices(IServiceCollection services)
        {
            //TODO parking
//            services.AddSingleton();
            services.AddDbContext<VehicleContext>(opt =>
                opt.UseInMemoryDatabase("VehicleList"));
            services.AddDbContext<TransactionContext>(opt =>
                opt.UseInMemoryDatabase("TransactionList"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP 
        //request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for 
                // production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller}/{action}/{id}");
                routes.MapRoute("default2", "{controller}/{action}");
                routes.MapRoute("default3",
                    "{controller}/{action}/{id}-{sum}"); //-{lastPaidTime}-{typeV}");
            });
        }
    }
}