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
        table.Columns.Add("X", typeof(string));
        table.Columns.Add("Y", typeof(string));

        int rowsCount = Math.Min(model.XValues.Length, model.YValues.Length);

        for (int i = 0; i < rowsCount; i++)
        {
            table.Rows.Add(
                model.Expression, 
                model.XValues[i].ToString(CultureInfo.InvariantCulture), 
                model.YValues[i].ToString(CultureInfo.InvariantCulture));
        }
        
        return table;
    }
}