using System.Drawing;

namespace FunctionToGraph.Models;

public class GraphModel
{
    public string ExpressionString { get; set; }
    public string FullExpression => "y=" + ExpressionString;
    public Color Color { get; set; }

    public override string ToString()
    {
        return FullExpression;
    }
}