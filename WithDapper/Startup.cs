using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WithDapper
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var connectionStringHolder = new ConnectionStringHolder(Configuration.GetConnectionString("DefaultConnection"));
            services.AddSingleton(connectionStringHolder);

            services.AddScoped<IMovieRepository, MovieRepository>(); //register the class in DI container.
            services.AddScoped<IFiddlerEx, FiddlerEx>(); //register the class in DI container.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();  //todo?? qq??

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

            //qq read this: https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-3.1#routing-mixed-ref-label
            app.UseEndpoints(endpoints =>
            {
                //todo more complete routing and its utilisation.

                //endpoints.MapControllerRoute(  
                //    name: "testing",
                //    pattern: "{controller}/{action=FiddlerExAddStudentMarksPost}",
                //    defaults: new { action = "Index" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");  //The trailing ? (in id?) indicates the id parameter is optional.

                //endpoints.MapControllerRoute(
                //    name: "api",
                //    pattern: "{controller}/{id?}");
            });
        }
    }
}
