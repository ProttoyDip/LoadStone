using Lodestone.ML.Models;
using Microsoft.ML;

namespace Lodestone.ML.Training;

/// <summary>End-to-end orchestrator: load -> engineer -> train -> evaluate -> save.</summary>
public class TrainingPipeline
{
    private readonly MLContext _mlContext;
    private readonly OuladDataLoader _loader;
    private readonly FeatureEngineering _features;
    private readonly ModelTrainer _trainer;
    private readonly ModelEvaluator _evaluator;

    public TrainingPipeline(MLContext mlContext, OuladDataLoader loader, FeatureEngineering features,
        ModelTrainer trainer, ModelEvaluator evaluator)
    {
        _mlContext = mlContext;
        _loader = loader;
        _features = features;
        _trainer = trainer;
        _evaluator = evaluator;
    }

    public ModelMetrics Run(string dataPath, string modelOutputPath)
        => throw new NotImplementedException();
}
