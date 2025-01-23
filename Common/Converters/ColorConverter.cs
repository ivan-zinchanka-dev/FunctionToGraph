using System.Drawing;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Common.Converters;

public class ColorConverter : DefaultTypeConverter
{
    public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        if (value is Color color)
        {
            return $"({color.R},{color.G},{color.B})";
        }
        return base.ConvertToString(value, row, memberMapData);
    }

    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return Color.Empty;
        }
        
        text = text.Trim('(', ')');
        
        string[] rawValues = text.Split(',', StringSplitOptions.RemoveEmptyEntries);

        if (rawValues.Length == 3 && 
            int.TryParse(rawValues[0].Trim(), out int r) &&
            int.TryParse(rawValues[1].Trim(), out int g) &&
            int.TryParse(rawValues[2].Trim(), out int b))
        {
            return Color.FromArgb(r, g, b);
        }

        throw new FormatException($"Invalid color format: {text}");
    }
}