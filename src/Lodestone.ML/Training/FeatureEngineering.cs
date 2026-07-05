using Microsoft.ML;

namespace Lodestone.ML.Training;

/// <summary>Builds the transformation pipeline (normalization, feature concat) for training/scoring.</summary>
public class FeatureEngineering
{
    private readonly MLContext _mlContext;

    public FeatureEngineering(MLContext mlContext) => _mlContext = mlContext;

    public IEstimator<ITransformer> BuildPipeline()
        => throw new NotImplementedException();
}
