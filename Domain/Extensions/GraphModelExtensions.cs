using System.Data;
using System.Globalization;
using Domain.Models;

namespace Domain.Extensions;

public static class GraphModelExtensions
{
    public static DataTable ToDataTable(this GraphModel model)
    {
        DataTable table = new DataTable();
        table.Locale = CultureInfo.InvariantCulture;
        table.Columns.Add("Expression", typeof(string));
        table.Columns.Add("X", typeof(double));
        table.Columns.Add("Y", typeof(double));

        int rowsCount = Math.Min(model.XValues.Length, model.YValues.Length);

        for (int i = 0; i < rowsCount; i++)
        {
            table.Rows.Add(model.Expression, model.XValues[i], model.YValues[i]);
        }
        
        return table;
    }
}