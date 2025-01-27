using System.Data;

namespace Domain.Csv;

public class CsvService
{
    private const char Separator = ',';
     
    public DataTable ReadData(string filePath)
    {
        DataTable data = new DataTable();
        
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line = reader.ReadLine();

            if (line != null)
            {
                string[] headers = line.Split(Separator);

                foreach (string header in headers)
                {
                    data.Columns.Add(header);
                }
            }

            while ((line = reader.ReadLine()) != null)
            {
                string[] fields = line.Split(Separator);
                data.Rows.Add(fields);
            }
        }

        return data;
    }
    
    public void WriteData(string filePath, DataTable data, bool append = false)
    {
        bool writeHeaders = MustWriteHeaders(filePath, append);
        
        using (StreamWriter writer = new StreamWriter(filePath, append))
        {
            if (writeHeaders)
            {
                writer.WriteLine(string.Join(Separator, GetHeaders(data.Columns)));
            }
            
            for (int i = 0; i < data.Rows.Count; i++)
            {
                writer.WriteLine(string.Join(Separator, GetFields(data.Rows[i], data.Columns.Count)));
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
                await writer.WriteLineAsync(string.Join(Separator, GetHeaders(data.Columns)));
            }
            
            for (int i = 0; i < data.Rows.Count; i++)
            {
                await writer.WriteLineAsync(string.Join(Separator, GetFields(data.Rows[i], data.Columns.Count)));
            }
        }
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