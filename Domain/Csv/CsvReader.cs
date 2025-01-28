using System.Data;

namespace Domain.Csv;

public class CsvReader
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
                ReadHeaders(line, data.Columns);
            }

            while ((line = reader.ReadLine()) != null)
            {
                ReadFields(line, data.Rows);
            }
        }

        return data;
    }
    
    public async Task<DataTable> ReadDataAsync(string filePath)
    {
        DataTable data = new DataTable();
        
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line = await reader.ReadLineAsync();

            if (line != null)
            {
                ReadHeaders(line, data.Columns);
            }

            while ((line = await reader.ReadLineAsync()) != null)
            {
                ReadFields(line, data.Rows);
            }
        }

        return data;
    }
    
    private void ReadHeaders(string line, DataColumnCollection dataColumns)
    {
        string[] headers = line.Split(Separator);

        foreach (string header in headers)
        {
            dataColumns.Add(header);
        }
    }

    private void ReadFields(string line, DataRowCollection dataRows)
    {
        string[] fields = line.Split(Separator);
        dataRows.Add(fields);
    }
}