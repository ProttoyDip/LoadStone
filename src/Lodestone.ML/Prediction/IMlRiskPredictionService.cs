using Lodestone.ML.Models;

namespace Lodestone.ML.Prediction;

/// <summary>
/// Inference contract owned by the ML layer. Application consumes this to keep ML.NET
/// out of the Application/Domain projects.
/// </summary>
public interface IMlRiskPredictionService
{
    RiskPrediction Predict(StudentActivityFeatures features);
}
