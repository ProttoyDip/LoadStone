using Lodestone.ML.Prediction;
using Lodestone.ML.Training;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;

namespace Lodestone.ML;

/// <summary>Registers ML.NET context, training components and the prediction service.</summary>
public static class DependencyInjection
{
    public static IServiceCollection AddMachineLearning(this IServiceCollection services, string modelPath)
    {
        services.AddSingleton(new MLContext(seed: 42));

        services.AddScoped<OuladDataLoader>();
        services.AddScoped<FeatureEngineering>();
        services.AddScoped<ModelTrainer>();
        services.AddScoped<ModelEvaluator>();
        services.AddScoped<TrainingPipeline>();

        services.AddRiskPredictionEngine(modelPath);
        services.AddScoped<IMlRiskPredictionService, MlRiskPredictionService>();

        return services;
    }
}
