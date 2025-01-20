using System.Drawing;
using System.Globalization;
using CsvHelper.Configuration.Attributes;

namespace FunctionCalculator;

[CultureInfo("InvariantCulture")]
public class CalculationRecord
{
    [Index(0), Name("Expression"), Optional]
    public string Expression { get; set; }
    
    [Index(1), Name("X")]
    public double XValue { get; set; }
    
    [Index(2), Name("Y")]
    public double YValue { get; set; }

    [Ignore]
    public Color GraphColor { get; set; } = Color.Empty;

    [Index(3), Name("Graph color"), Optional] 
    public string ConvertedGraphColor => ConvertColor(GraphColor);

    public CalculationRecord()
    {
        Expression = string.Empty;
        XValue = 0;
        YValue = 0;
    }
    
    public CalculationRecord(string expression, double xValue, double yValue, Color graphColor)
    {
        Expression = expression;
        XValue = xValue;
        YValue = yValue;
        GraphColor = graphColor;
    }

    public CalculationRecord(double xValue, double yValue)
    {
        Expression = string.Empty;
        XValue = xValue;
        YValue = yValue;
    }

    private static string ConvertColor(Color color)
    {
        return color.IsEmpty ? string.Empty : $"{color.R}, {color.G}, {color.B}";
    }

    public override string ToString()
    {
        return $"({Expression}, {XValue.ToString(CultureInfo.InvariantCulture)}, {YValue.ToString(CultureInfo.InvariantCulture)}, {ConvertedGraphColor})";
    }
}