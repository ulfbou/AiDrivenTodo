using Microsoft.ML.Data;
using Microsoft.ML;

namespace AiDrivenTodo;

using Microsoft.ML.Data;
using Microsoft.ML;

public class TaskClassification
{
    private readonly MLContext _mlContext;
    private readonly ITransformer _model;

    public TaskClassification()
    {
        _mlContext = new MLContext();

        var dataPath = Path.Combine(Environment.CurrentDirectory, "tasks.csv");
        var data = _mlContext.Data.LoadFromTextFile<TaskData>(dataPath, hasHeader: true, separatorChar: ',');

        var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(TaskData.Description))
            .Append(_mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(TaskData.PriorityLevel)))
            .Append(_mlContext.Transforms.Concatenate("Features", "Features"))
            .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(labelColumnName: "Label", featureColumnName: "Features"))
            .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "Label"));

        _model = pipeline.Fit(data);
    }

    public string PredictPriority(string description)
    {
        var predictionEngine = _mlContext.Model.CreatePredictionEngine<TaskData, TaskPrediction>(_model);
        var prediction = predictionEngine.Predict(new TaskData { Description = description });

        // Set a confidence threshold (adjust as needed)
        double confidenceThreshold = 0.8;

        return !string.IsNullOrEmpty(prediction.PredictedLabel) ? prediction.PredictedLabel : "Unknown";
    }

    public class TaskData
    {
        [LoadColumn(0)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public string Description { get; set; }

        [LoadColumn(1)]
        public string PriorityLevel { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }

    public class TaskPrediction : TaskData
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public string PredictedLabel { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }
}