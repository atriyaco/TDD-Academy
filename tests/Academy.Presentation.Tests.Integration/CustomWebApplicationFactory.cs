using System;
using System.Linq;
using Academy.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Academy.Presentation.Tests.Integration
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var connectionString =
                "Data Source=.;Initial Catalog=TddAcademy;Persist Security Info=True;User ID=sa;Password=123456";

            if (environment == "Staging")
                connectionString =
                    "Data Source=185.88.152.127,1430;Initial Catalog=1768_tddacademy;Persist Security Info=True;User ID=1768_tddacademy;Password=Hh@123456";

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<AcademyContext>));
                services.Remove(descriptor);
                services.AddDbContext<AcademyContext>(options => { options.UseSqlServer(connectionString); });
            });
        }
    }
}