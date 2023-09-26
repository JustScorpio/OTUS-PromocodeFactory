using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Otus.Teaching.PromoCodeFactory.WebHost
{
    public class Startup
    {
        private IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //I DONT KNWO HOW TO AVOID THIZ
            services.AddDbContext<AppDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("SqliteConnectionString")));
            var contextOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseLazyLoadingProxies()
                .UseSqlite(Configuration.GetConnectionString("SqliteConnectionString"));

            var context = new AppDbContext(contextOptionsBuilder.Options);

            services.AddSingleton(typeof(IRepository<Employee>), (x) => new EfRepository<Employee>(context));
            services.AddSingleton(typeof(IRepository<Role>), (x) => new EfRepository<Role>(context));
            services.AddSingleton(typeof(IRepository<Preference>), (x) => new EfRepository<Preference>(context));
            services.AddSingleton(typeof(IRepository<Customer>), (x) => new EfRepository<Customer>(context));
            services.AddSingleton(typeof(IRepository<CustomerPreference>), (x) => new EfRepository<CustomerPreference>(context));

            services.AddOpenApiDocument(options =>
                {
                    options.Title = "PromoCode Factory API Doc";
                    options.Version = "1.0";
                });
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
                app.UseHsts();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3(x =>
            {
                x.DocExpansion = "list";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}