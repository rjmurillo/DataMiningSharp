using System.Collections.Generic;
using System.Linq;

namespace DataMiningSharp.Core.DataClassifier
{
    public static class DataClassifierFilterExtensions
    {
        public static bool AllSatisfied<TClassifier>(this IEnumerable<IDataClassifierFilter<TClassifier>> filters,
            TClassifier dataClassifier) where TClassifier : IDataClassifierResult
        {
            return ((filters == null) || filters.All(filter => filter.IsSatisfied(dataClassifier)));
        }


    }
}