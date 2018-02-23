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
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Swashbuckle.AspNetCore.Swagger;
using BikeRentalService.Models;

namespace BikeRentalService
{
    public class Startup
    {
        private BikeRentalContext context = new BikeRentalContext();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            bool enableTestdata = true;

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

            var allCustomers = context.Customers.Select(x => x).ToList();
            if (allCustomers.Count() == 0 && enableTestdata)
            {
                context.Customers.Add(new Customer()
                {
                    Birthday = new DateTime(1998, 9, 7),
                    FirstName="Maurice",
                    LastName="el-Banna",
                    Gender=context.PersonGenders.Where(g=>g.GenderId==1).FirstOrDefault(),
                    HouseNumber=29,
                    Street="Pegasusweg",
                    Town="Linz",
                    ZipCode=4030
                });
                context.SaveChanges();
            }

            var allBikes = context.Bikes.Select(x => x).ToList();
            if (allBikes.Count() == 0 && enableTestdata)
            {
                context.Bikes.Add(new Bike()
                {
                    Brand="SomeBrand",
                    Category=context.BikeCategorys.Where(c=>c.CategoryId==1).FirstOrDefault(),
                    DateOfLastService=new DateTime(2018, 2, 17, 1, 0, 0),
                    Notes="It's a bike",
                    PurchaseDate=new DateTime(2018, 2, 16, 0, 0, 0),
                    RentalPriceFirstHour=2,
                    RentalPriceExtraHour=5
                });
                context.SaveChanges();
            }

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
