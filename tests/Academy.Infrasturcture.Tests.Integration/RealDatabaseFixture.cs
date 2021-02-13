using System;
using System.Transactions;
using Academy.Domain.Tests.Unit.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Academy.Infrastructure.Tests.Integration
{
    public class RealDatabaseFixture : IDisposable
    {
        public AcademyContext Context;
        private readonly TransactionScope _scope;
        
        public RealDatabaseFixture()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var connectionString =
                "Data Source=.;Initial Catalog=TddAcademy;Persist Security Info=True;User ID=sa;Password=123456";
                
            if (environment == "Staging")
                connectionString =
                    "Data Source=185.88.152.127,1430;Initial Catalog=1768_tdd_academy;Persist Security Info=True;User ID=1768_tdd_academy;Password=Hh@123456";

            Console.WriteLine(connectionString);

            var options = new DbContextOptionsBuilder<AcademyContext>()
                .UseSqlServer(connectionString).Options;
            Context = new AcademyContext(options);
            _scope = new TransactionScope();
            var builder = new CourseTestBuilder();

            var asp = builder
                .WithName("ASP.NET Core 5")
                .WithTuition(780)
                .WithInstructor("Hossein")
                .Build();
            var git = builder
                .WithName("Git")
                .WithTuition(120)
                .WithInstructor("Hossein")
                .Build();
            var webDesign = builder
                .WithName("Web Design")
                .WithTuition(320)
                .WithInstructor("Mohammad")
                .Build();

            Context.Add(asp);
            Context.Add(git);
            Context.Add(webDesign);
            Context.SaveChanges();
        }

        public void Dispose()
        {
            _scope.Dispose();
            Context.Database.ExecuteSqlRaw("truncate table [TddAcademy].[dbo].[Courses]");
            Context.Database.ExecuteSqlRaw("truncate table [TddAcademy].[dbo].[Sections]");
            Context.SaveChanges();
            Context.Dispose();
        }
    }
}