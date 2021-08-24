using System;
using System.Collections.Generic;

namespace GCommon.Differencing
{
    public class Comparison<T>
    {
        public T First { get; set; }
        public T Second { get; set; }
        public DateTime LastRan { get; set; }
        public IEnumerable<Difference<T>> Differences { get; set; }

        public void Run(T first, T second)
        {
            
        }

        public T Merge()
        {
            return default;
        }
    }
}