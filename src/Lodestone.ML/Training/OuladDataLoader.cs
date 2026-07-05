using Lodestone.ML.Models;
using Microsoft.ML;

namespace Lodestone.ML.Training;

/// <summary>Loads the OULAD dataset (or exported activity logs) into an ML.NET IDataView.</summary>
public class OuladDataLoader
{
    private readonly MLContext _mlContext;

    public OuladDataLoader(MLContext mlContext) => _mlContext = mlContext;

    public IDataView Load(string csvPath)
        => _mlContext.Data.LoadFromTextFile<StudentActivityFeatures>(csvPath, hasHeader: true, separatorChar: ',');
}
