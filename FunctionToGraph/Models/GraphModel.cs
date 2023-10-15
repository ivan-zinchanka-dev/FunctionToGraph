using System.Drawing;

namespace FunctionToGraph.Models;

public class GraphModel
{
    public string ExpressionString { get; private set; }
    public double[] XValues { get; private set; }
    public double[] YValues { get; private set; }
    public Color Color { get; private set; }

    public string FullExpression => "y=" + ExpressionString;
    
    public GraphModel(string expressionString, double[] xValues, double[] yValues, Color color)
    {
        ExpressionString = expressionString;
        XValues = xValues;
        YValues = yValues;
        Color = color;
    }

    public override string ToString()
    {
        return FullExpression;
    }
}