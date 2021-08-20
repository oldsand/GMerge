using System;
using System.Collections.Generic;

namespace GCommon.Differencing
{
    public class Comparison<T>
    {
        
        public DateTime LastRan { get; set; }
        public IEnumerable<Difference> Differences { get; set; }

        public void RunOn(T first, T second)
        {
            
        }

        public T Merge()
        {
            return default;
        }
    }
}