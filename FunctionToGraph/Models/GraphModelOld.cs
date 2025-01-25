using System.Drawing;
using Newtonsoft.Json;

namespace FunctionToGraph.Models;

// TODO Delete
public class GraphModelOld
{
    public string Expression { get; private set; }
    public double[] XValues { get; private set; }
    public double[] YValues { get; private set; }
    public Color Color { get; private set; }

    [JsonIgnore] public string FullExpression => "y=" + Expression;
    
    public GraphModelOld(string expression, double[] xValues, double[] yValues, Color color)
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