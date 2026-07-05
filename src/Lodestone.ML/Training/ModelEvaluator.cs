using Lodestone.ML.Models;
using Microsoft.ML;

namespace Lodestone.ML.Training;

/// <summary>Computes metrics on a held-out test split.</summary>
public class ModelEvaluator
{
    private readonly MLContext _mlContext;

    public ModelEvaluator(MLContext mlContext) => _mlContext = mlContext;

    public ModelMetrics Evaluate(ITransformer model, IDataView testData)
        => throw new NotImplementedException();
}
