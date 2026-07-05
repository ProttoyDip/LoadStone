using FluentAssertions;
using Xunit;

namespace Lodestone.IntegrationTests.Database;

public class DbInitializerTests
{
    [Fact]
    public void Placeholder_database_test() => true.Should().BeTrue();
}
