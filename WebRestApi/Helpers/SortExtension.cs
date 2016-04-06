using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;

namespace WebRestApi.Helpers
{
    public static class SortExtension
    {
        public static IEnumerable<T> ApplySort<T>(this IEnumerable<T> source, string sort)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (sort == null)
            {
                return source;
            }

            // split the sort string
            var lstSort = sort.Split(',');

            // run through the sorting options and apply them - in reverse
            // order, otherwise results will come out sorted by the last 
            // item in the string first!
            foreach (var sortOption in lstSort.Reverse())
            {
                // if the sort option starts with "-", we order
                // descending, ortherwise ascending

                if (sortOption.StartsWith("-"))
                {
                    source = source.OrderBy(sortOption.Remove(0, 1) + " descending");
                }
                else
                {
                    source = source.OrderBy(sortOption);
                }

            }

            return source;
        }
    }
}