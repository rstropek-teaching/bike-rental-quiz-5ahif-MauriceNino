using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BikeRentalService.Models;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

namespace BikeRentalService
{
    public class Startup
    {
        private BikeRentalContext context = new BikeRentalContext();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var allPersonGenders=context.PersonGenders.Select(x => x).ToList();
            if (allPersonGenders.Count() == 0)
            {
                context.PersonGenders.Add(new PersonGender()
                {
                    GenderName = "Male"
                });
                context.PersonGenders.Add(new PersonGender()
                {
                    GenderName = "Female"
                });
                context.PersonGenders.Add(new PersonGender()
                {
                    GenderName = "Unknown"
                });
                context.SaveChanges();
            }

            var allBikeCategories = context.BikeCategorys.Select(x => x).ToList();
            if (allBikeCategories.Count() == 0)
            {
                context.BikeCategorys.Add(new BikeCategory()
                {
                    CategoryName = "Standard Bike"
                });
                context.BikeCategorys.Add(new BikeCategory()
                {
                    CategoryName = "Mountainbike"
                });
                context.BikeCategorys.Add(new BikeCategory()
                {
                    CategoryName = "Trecking Bike"
                });
                context.BikeCategorys.Add(new BikeCategory()
                {
                    CategoryName = "Racing Bike"
                });
                context.SaveChanges();
            }

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
