using System;

namespace Academy.Domain.Tests.Unit.ClassFixtures
{
    public class IdentifierFixture : IDisposable
    {
        public static Guid Id { get; set; }

        public IdentifierFixture()
        {
            Id = Guid.NewGuid();
        }

        public void Dispose()
        {
            Id = Guid.Empty;
        }
    }
}