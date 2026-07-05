using FluentAssertions;
using Lodestone.ML.Models;
using Xunit;

namespace Lodestone.MLTests;

public class ModelEvaluationTests
{
    [Fact]
    public void Metrics_default_to_zero()
        => new ModelMetrics().Accuracy.Should().Be(0);
}
