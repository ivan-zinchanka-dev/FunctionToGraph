namespace FunctionDomain.Models
{
    public struct Range
    {
        public double Min { get; }
        public double Max { get; }
        public int Count { get; }

        public Range(double min, double max, int count)
        {
            if (max < min)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, "Should be greater than " + nameof(min));
            }
        
            if (count < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "Should be greater than 2");
            }
        
            Min = min;
            Max = max;
            Count = count;
        }
    
        public IEnumerable<double> Generate()
        {
            List<double> values = new List<double>(Count + 1);
            double delta = (Max - Min) / Count;
            values.Add(Min);
        
            for (int i = 1; i < Count + 1; i++)
            {
                values.Add(values[i - 1] + delta);
            }
        
            return values;
        }

    }
}


