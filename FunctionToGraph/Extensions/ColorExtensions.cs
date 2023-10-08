using WPFColor = System.Windows.Media.Color;
using DNETColor = System.Drawing.Color;

namespace FunctionToGraph.Extensions;

public static class ColorExtensions
{
    public static WPFColor ToWpfColor(this DNETColor color)
    {
        return WPFColor.FromArgb(color.A, color.R, color.G, color.B);
    }
    
    public static DNETColor ToDotNetColor(this WPFColor color)
    {
        return DNETColor.FromArgb(color.A, color.R, color.G, color.B);
    }
    
}