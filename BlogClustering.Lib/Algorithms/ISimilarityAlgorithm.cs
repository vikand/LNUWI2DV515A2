using System.Collections.Generic;

namespace BlogClustering.Lib.Algorithms
{
    public interface ISimilarityAlgorithm
    {
        double CalculateSimilarity<TKey>(IDictionary<TKey, double> valuesA, IDictionary<TKey, double> valuesB);
    }
}
