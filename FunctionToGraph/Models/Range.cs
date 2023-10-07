using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FunctionToGraph.Models;

public struct Range
{
    public double Start { get; private set; }
    public double End { get; private set; }
    public int Count { get; private set; }

    public Range(double start, double end, int count)
    {
        if (count < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(count), count, "Should be > 2");
        }

        Start = start;
        End = end;
        Count = count;
    }
    
    public IEnumerable<double> Generate()
    {
        List<double> values = new List<double>(Count + 1);
        double delta = (End - Start) / Count;
        values.Add(Start);
        
        for (int i = 1; i < Count + 1; i++)
        {
            values.Add(values[i - 1] + delta);
        }
        
        return values;
    }

}
