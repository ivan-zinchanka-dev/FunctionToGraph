using System.Data;

namespace Domain.Csv;

public class CsvWriter
{
    private const char Separator = ',';
    
    public void WriteData(string filePath, DataTable data, bool append = false)
    {
        bool writeHeaders = MustWriteHeaders(filePath, append);
        
        using (StreamWriter writer = new StreamWriter(filePath, append))
        {
            if (writeHeaders)
            {
                writer.WriteLine(GetHeaderLine(data.Columns));
            }
            
            for (int i = 0; i < data.Rows.Count; i++)
            {
                writer.WriteLine(GetFieldLine(data.Rows[i], data.Columns.Count));
            }
        }
    }
    
    public async Task WriteDataAsync(string filePath, DataTable data, bool append = false)
    {
        bool writeHeaders = MustWriteHeaders(filePath, append);
        
        using (StreamWriter writer = new StreamWriter(filePath, append))
        {
            if (writeHeaders)
            {
                await writer.WriteLineAsync(GetHeaderLine(data.Columns));
            }
         
            for (int i = 0; i < data.Rows.Count; i++)
            {
                await writer.WriteLineAsync(GetFieldLine(data.Rows[i], data.Columns.Count));
            }
        }
    }

    private string GetHeaderLine(DataColumnCollection columns)
    {
        return string.Join(Separator, GetHeaders(columns));
    }

    private string GetFieldLine(DataRow row, int columnsCount)
    {
        return string.Join(Separator, GetFields(row, columnsCount));
    }
    
    private bool MustWriteHeaders(string filePath, bool append)
    {
        if (append)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return !(fileInfo.Exists && fileInfo.Length > 0);
        }
        else
        {
            return true;
        }
    }
    
    private string[] GetHeaders(DataColumnCollection dataColumns)
    {
        string[] headers = new string[dataColumns.Count];

        for (int i = 0; i < headers.Length; i++)
        {
            headers[i] = dataColumns[i].ColumnName;
        }

        return headers;
    }
    
    private string[] GetFields(DataRow dataRow, int columnsCount)
    {
        string[] fields = new string[columnsCount];

        for (int j = 0; j < fields.Length; j++)
        {
            fields[j] = dataRow[j].ToString();
        }
        
        return fields;
    }
}