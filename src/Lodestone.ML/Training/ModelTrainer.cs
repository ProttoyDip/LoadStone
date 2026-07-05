using Microsoft.ML;

namespace Lodestone.ML.Training;

/// <summary>Trains the binary classification model and persists it to SavedModels.</summary>
public class ModelTrainer
{
    private readonly MLContext _mlContext;

    public ModelTrainer(MLContext mlContext) => _mlContext = mlContext;

    public ITransformer Train(IDataView trainingData, IEstimator<ITransformer> pipeline)
        => throw new NotImplementedException();

    public void Save(ITransformer model, DataViewSchema schema, string outputPath)
        => throw new NotImplementedException();
}
