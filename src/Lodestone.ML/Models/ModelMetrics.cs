namespace Lodestone.ML.Models;

/// <summary>Evaluation metrics captured after training.</summary>
public class ModelMetrics
{
    public double Accuracy { get; set; }
    public double AreaUnderRocCurve { get; set; }
    public double F1Score { get; set; }
    public double Precision { get; set; }
    public double Recall { get; set; }
    public string? ModelVersion { get; set; }
}
