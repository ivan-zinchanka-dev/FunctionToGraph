using CsvHelper.Configuration.Attributes;

namespace FunctionCalculator;

public class CalculationRecord
{
    [Index(0), Name("X")]
    public double XValue { get; set; }
    
    [Index(1), Name("Y")]
    public double YValue { get; set; }

    public CalculationRecord(double xValue, double yValue)
    {
        XValue = xValue;
        YValue = yValue;
    }
}