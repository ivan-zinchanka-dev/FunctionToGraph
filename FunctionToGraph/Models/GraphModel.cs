using System.Drawing;

namespace FunctionToGraph.Models;

public class GraphModel
{
    public string ExpressionString { get; set; }
    public string FullExpression => "y=" + ExpressionString;
    
    public double[] XValues { get; set; }
    public double[] YValues { get; set; }
    
    public Color Color { get; set; }

    public override string ToString()
    {
        return FullExpression;
    }
}