﻿using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain.CsvService;

public static class CsvReader
{
    private const char Separator = ',';
    private const string RegexPattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
    
    public static DataTable ReadData(string filePath)
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
    
    public static async Task<DataTable> ReadDataAsync(string filePath)
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
    
    private static void ReadHeaders(string line, DataColumnCollection dataColumns)
    {
        string[] headers = RemoveDoubledInnerQuotesIfNeed(
            RemoveOuterQuotes(line.Split(Separator)));

        foreach (string header in headers)
        {
            dataColumns.Add(header);
        }
    }

    private static void ReadFields(string line, DataRowCollection dataRows)
    {
        string[] fields = RemoveDoubledInnerQuotesIfNeed(
            RemoveOuterQuotes(Regex.Split(line, RegexPattern)));
        dataRows.Add(fields);
    }
    
    private static string[] RemoveOuterQuotes(string[] sources)
    {
        return sources.Select(RemoveOuterQuotes).ToArray();
    }
    
    private static string RemoveOuterQuotes(string source)
    {
        return source.Trim('"');
    }
    
    private static string[] RemoveDoubledInnerQuotesIfNeed(string[] sources)
    {
        return sources.Select(RemoveDoubledInnerQuotesIfNeed).ToArray();
    }
    
    private static string RemoveDoubledInnerQuotesIfNeed(string source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        StringBuilder result = new StringBuilder(source.Length);
        bool lastWasQuote = false;
        
        foreach (char c in source)
        {
            if (c == '"' && lastWasQuote)
            {
                lastWasQuote = false;
            }
            else
            {
                result.Append(c);
                lastWasQuote = (c == '"');
            }
        }

        return result.ToString();
    }
    
}