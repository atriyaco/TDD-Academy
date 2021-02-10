using System;
using System.Transactions;
using Academy.Domain.Tests.Unit.Builders;
using Microsoft.EntityFrameworkCore;

namespace Academy.Infrastructure.Tests.Integration
{
    public class RealDatabaseFixture : IDisposable
    {
        public AcademyContext Context;
        private readonly TransactionScope _scope;

        public RealDatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<AcademyContext>()
                .UseSqlServer(
                    "Data Source=.;Initial Catalog=TddAcademy;Persist Security Info=True;User ID=sa;Password=123456")
                .Options;
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