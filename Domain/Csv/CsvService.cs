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


    private static bool MustWriteHeaders(string filePath, bool append)
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

    public void WriteData(string filePath, DataTable data, bool append = false)
    {
        bool writeHeaders = MustWriteHeaders(filePath, append);
        
        using (StreamWriter writer = new StreamWriter(filePath, append))
        {
            if (writeHeaders)
            {
                string[] headers = new string[data.Columns.Count];

                for (int i = 0; i < headers.Length; i++)
                {
                    headers[i] = data.Columns[i].ColumnName;
                }
            
                writer.WriteLine(string.Join(Separator, headers));
            }
            
            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow row = data.Rows[i];
                string[] fields = new string[data.Columns.Count];

                for (int j = 0; j < fields.Length; j++)
                {
                    fields[j] = row[j].ToString();
                }
                
                writer.WriteLine(string.Join(Separator, fields));
            }
            
            writer.Flush();
        }
        
    }
    
    
    public async Task WriteDataAsync(string filePath, DataTable data, bool append = false)
    {
        using (StreamWriter writer = new StreamWriter(filePath, append))
        {
            string[] headers = new string[data.Columns.Count];

            for (int i = 0; i < headers.Length; i++)
            {
                headers[i] = data.Columns[i].ColumnName;
            }
            
            await writer.WriteAsync(string.Join(Separator, headers));

            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow row = data.Rows[i];
                string[] fields = new string[data.Columns.Count];

                for (int j = 0; j < fields.Length; j++)
                {
                    fields[j] = row[j].ToString();
                }
                
                await writer.WriteAsync(string.Join(Separator, fields));
            }
            
        }
        
    }
}