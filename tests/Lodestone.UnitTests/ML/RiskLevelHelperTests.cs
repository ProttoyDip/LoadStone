using FluentAssertions;
using Lodestone.Shared.Helpers;
using Xunit;

namespace Lodestone.UnitTests.ML;

public class RiskLevelHelperTests
{
    [Theory]
    [InlineData(0.10, "Low")]
    [InlineData(0.40, "Moderate")]
    [InlineData(0.60, "High")]
    [InlineData(0.90, "Critical")]
    public void Describes_probability_bucket(double probability, string expected)
        => RiskLevelHelper.DescribeProbability(probability).Should().Be(expected);
}
