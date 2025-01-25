using System.Drawing;

namespace Domain.Models;

public class GraphModel
{
    public string Expression { get; private set; }
    public double[] XValues { get; private set; }
    public double[] YValues { get; private set; }
    public Color Color { get; private set; }

    public string FullExpression => "y=" + Expression;
    
    public GraphModel(string expression, double[] xValues, double[] yValues)
    {
        Expression = expression;
        XValues = xValues;
        YValues = yValues;
    }
    
    public GraphModel(string expression, double[] xValues, double[] yValues, Color color)
    {
        Expression = expression;
        XValues = xValues;
        YValues = yValues;
        Color = color;
    }

    public override string ToString()
    {
        return FullExpression;
    }
}