using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FunctionToGraph.Models;

public struct IntegerRange
{
    public int Start { get; private set; }
    public int End { get; private set; }

    public IntegerRange(int startInclusive, int endExclusive)
    {
        Start = startInclusive;
        End = endExclusive;
    }
    
    public IEnumerable<int> Generate()
    {
        return Enumerable.Range(Start, End - Start);
    }

}
