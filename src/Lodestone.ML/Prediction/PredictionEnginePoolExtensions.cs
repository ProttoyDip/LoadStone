using Lodestone.ML.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;

namespace Lodestone.ML.Prediction;

public static class PredictionEnginePoolExtensions
{
    /// <summary>Registers the pooled prediction engine bound to the saved risk model.</summary>
    public static IServiceCollection AddRiskPredictionEngine(this IServiceCollection services, string modelPath)
    {
        services.AddPredictionEnginePool<StudentActivityFeatures, RiskPrediction>()
            .FromFile(modelName: "RiskModel", filePath: modelPath, watchForChanges: true);

        return services;
    }
}
