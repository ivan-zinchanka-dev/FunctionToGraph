using CsvHelper.Configuration.Attributes;

namespace FunctionCalculator;

[CultureInfo("InvariantCulture")]
public class CalculationRecord
{
    [Index(0), Name("Expression")]
    public string Expression { get; set; }
    
    [Index(1), Name("X")]
    public double XValue { get; set; }
    
    [Index(2), Name("Y")]
    public double YValue { get; set; }
    
    public CalculationRecord()
    {
        Expression = string.Empty;
        XValue = 0;
        YValue = 0;
    }
    
    public CalculationRecord(string expression, double xValue, double yValue)
    {
        Expression = expression;
        XValue = xValue;
        YValue = yValue;
    }

    public override string ToString()
    {
        return $"{Expression}: ({XValue:f3}; {YValue:f3})";
    }
}