using System.Data;
using System.Globalization;
using FunctionDomain.Models;

namespace FunctionDomain.Extensions;

public static class GraphModelExtensions
{
    private static readonly IFormatProvider ValueFormat = CultureInfo.InvariantCulture; 
    
    public static DataTable ToDataTable(this GraphModel model)
    {
        DataTable table = new DataTable();
        table.Columns.Add("Expression", typeof(string));
        table.Columns.Add("X", typeof(string));
        table.Columns.Add("Y", typeof(string));

        int rowsCount = Math.Min(model.XValues.Length, model.YValues.Length);

        for (int i = 0; i < rowsCount; i++)
        {
            table.Rows.Add(
                model.Expression, 
                model.XValues[i].ToString(ValueFormat), 
                model.YValues[i].ToString(ValueFormat));
        }
        
        return table;
    }
}