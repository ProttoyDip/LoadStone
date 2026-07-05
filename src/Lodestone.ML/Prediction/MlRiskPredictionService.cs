using Lodestone.ML.Models;
using Microsoft.Extensions.ML;

namespace Lodestone.ML.Prediction;

/// <summary>Uses a pooled ML.NET prediction engine to score a student's features.</summary>
public class MlRiskPredictionService : IMlRiskPredictionService
{
    private readonly PredictionEnginePool<StudentActivityFeatures, RiskPrediction> _pool;

    public MlRiskPredictionService(PredictionEnginePool<StudentActivityFeatures, RiskPrediction> pool)
        => _pool = pool;

    public RiskPrediction Predict(StudentActivityFeatures features)
        => _pool.Predict(modelName: "RiskModel", example: features);
}
