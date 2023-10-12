using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinderImport
{
    public static class StringListExtensions
    {
        public static IEnumerable<T> RealDistinct<T>(this IEnumerable<T> source)
        {
            List<T> uniques = new List<T>();
            foreach (T item in source)
            {
                if (!uniques.Contains(item)) uniques.Add(item);
            }
            return uniques;
        }
    }
}
