using FluentAssertions;
using Lodestone.ML.Models;
using Xunit;

namespace Lodestone.MLTests;

public class RiskPredictionServiceTests
{
    [Fact]
    public void Features_hold_assigned_values()
    {
        var features = new StudentActivityFeatures { LoginFrequency = 3f };
        features.LoginFrequency.Should().Be(3f);
    }
}
