using System.Data;
using Domain.Models;

namespace Domain.Factories;

public static class GraphModelParser
{
    private const string ExpressionHeader = "Expression";
    private const string XHeader = "X";
    private const string YHeader = "Y";

    private readonly struct Record
    {
        public string Expression { get; }
        public double XValue { get; }
        public double YValue { get; }

        public Record(string expression, double xValue, double yValue)
        {
            Expression = expression;
            XValue = xValue;
            YValue = yValue;
        }
    }
    
    public static IEnumerable<GraphModel> Parse(DataTable table)
    {
        Record[] records = new Record[table.Rows.Count];
        
        for (int i = 0; i < table.Rows.Count; i++)
        {
            DataRow dataRow = table.Rows[i];
            records[i] = new Record(
                dataRow[ExpressionHeader].ToString(), 
                double.Parse(dataRow[XHeader].ToString()), 
                double.Parse(dataRow[YHeader].ToString()));
        }

        IEnumerable<IGrouping<string, Record>> groups = records.GroupBy(record => record.Expression);
        List<GraphModel> models = new List<GraphModel>();
        
        foreach (IGrouping<string, Record> group in groups)
        {
            models.Add(Parse(group.Key, group.ToArray()));
        }

        return models;
    }

    private static GraphModel Parse(string key, Record[] records)
    {
        double[] xValues = new double[records.Length];
        double[] yValues = new double[records.Length];
        
        for (int i = 0; i < records.Length; i++)
        {
            xValues[i] = records[i].XValue;
            yValues[i] = records[i].YValue;
        }

        return new GraphModel(key, xValues, yValues);
    }
}