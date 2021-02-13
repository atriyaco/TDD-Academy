using Xunit;

namespace Academy.Domain.Tests.Unit.CollectionFixtures
{
    [CollectionDefinition("Database Collection")]
    public class DatabaseFixtureDefinition : ICollectionFixture<DatabaseFixture>
    {
    }
}
