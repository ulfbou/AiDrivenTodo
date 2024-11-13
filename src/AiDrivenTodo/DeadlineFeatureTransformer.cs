using Microsoft.ML.Data;
using Microsoft.ML;
using static AiDrivenTodo.TaskClassification;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;

namespace AiDrivenTodo;

#if false
public class DeadlineFeatureTransformer : ITransformer, IEstimator<DeadlineFeatureTransformer>
{
    private DataViewSchema outputSchema;

    public DeadlineFeatureTransformer(DataViewSchema outputSchema) => this.outputSchema = outputSchema;

    public bool IsRowToRowMapper => throw new NotImplementedException();

    // Fit the input data to the transformer
    public DeadlineFeatureTransformer Fit(IDataView input)
    {
        var inputSchema = input.Schema;
        var descriptionColumn = inputSchema.GetColumnOrNull(nameof(TaskData.Description));
        var outputSchema = Shape(inputSchema);
        return new DeadlineFeatureTransformer(outputSchema);
    }
    public DataViewSchema GetOutputSchema(DataViewSchema inputSchema)
    {
        return outputSchema;
    }
    public SchemaShape GetOutputSchema(SchemaShape inputSchema)
    {
        var featureColumn = new VectorDataViewType(TextDataViewType.Instance);
        var outputColumns = new List<SchemaShape.Column>
        {
            new SchemaShape.Column("DeadlineFeature", SchemaShape.Column.VectorKind.Vector, featureColumn, false)
        };
        return new SchemaShape(outputColumns);
    }
    public IRowToRowMapper GetRowToRowMapper(DataViewSchema inputSchema) => throw new NotImplementedException();
    public void Save(ModelSaveContext ctx) => throw new NotImplementedException();

    public DataViewSchema Shape(DataViewSchema inputSchema)
    {
        var featureColumn = new VectorDataViewType(TextDataViewType.Instance);
        var outputSchema = new DataViewSchema.Builder()
            .AddColumn(nameof(TaskData.Description), inputSchema[nameof(TaskData.Description)].Type)
            .AddColumn("DeadlineFeature", featureColumn)
            .ToSchema();
        return outputSchema;
    }

    public IDataView Transform(IDataView input) => throw new NotImplementedException();

    public class Transformer : ITransformer
    {
        private readonly DataViewSchema _outputSchema;

        public Transformer(DataViewSchema outputSchema) => _outputSchema = outputSchema;

        public DataViewSchema GetOutputSchema(DataViewSchema inputSchema) => _outputSchema;
        public IRowToRowMapper GetRowToRowMapper(DataViewSchema inputSchema) => throw new NotImplementedException();
        public void Save(ModelSaveContext ctx) => throw new NotImplementedException();
        public bool IsRowToRowMapper => throw new NotImplementedException();

        public IDataView Transform(IDataView input)
        {
            var inputSchema = input.Schema;
            var descriptionColumn = inputSchema.GetColumnOrNull(nameof(TaskData.Description));
            var outputSchema = _outputSchema;

            return new DelegateDataView(
                input,
                GetRow,
                outputSchema);

            void GetRow(ref DataViewRowCursor cursor, ref VBuffer<float> feature)
            {
                var description = cursor.GetGetter<ReadOnlyMemory<char>>(descriptionColumn).Invoke();
                var hasDeadline = Regex.IsMatch(description.ToString(), @"deadline|due|by|until");
                feature = hasDeadline ? new VBuffer<float>(1, new[] { 1f }) : new VBuffer<float>(1, new[] { 0f });
            }
        }
    }
}
#endif