using System;
using System.Collections.Generic;
using System.Linq;

namespace BlogClustering.Lib.Algorithms
{
    public class PearsonCorrelationSimilarityAlgorithm : ISimilarityAlgorithm
    {
        public double CalculateSimilarity<TKey>(IDictionary<TKey, double> valuesA, IDictionary<TKey, double> valuesB)
        {
            var matchingKeys = valuesA.Keys.Where(valuesB.ContainsKey);
            var numberOfMatchingKeys = matchingKeys.Count();

            if (numberOfMatchingKeys == 0)
                return 0;

            double sumOfAllValuesForA = 0,
                   sumOfAllValuesForB = 0,
                   sumOfSquaresOfValuesForA = 0,
                   sumOfSquaresOfValuesForB = 0,
                   sumProductOfValuesForAandB = 0;

            foreach (var key in matchingKeys)
            {
                var valueA = valuesA[key];
                var valueB = valuesB[key];

                sumOfAllValuesForA += valueA;
                sumOfAllValuesForB += valueB;
                sumOfSquaresOfValuesForA += valueA * valueA;
                sumOfSquaresOfValuesForB += valueB * valueB;
                sumProductOfValuesForAandB += valueA * valueB;
            }

            var numerator = sumProductOfValuesForAandB -
                            (sumOfAllValuesForA * sumOfAllValuesForB / numberOfMatchingKeys);
            var denominator = Math.Sqrt(
                (sumOfSquaresOfValuesForA - sumOfAllValuesForA * sumOfAllValuesForA / numberOfMatchingKeys) *
                (sumOfSquaresOfValuesForB - sumOfAllValuesForB * sumOfAllValuesForB / numberOfMatchingKeys));

            return denominator == 0 || numerator < 0 ? 0 : numerator / denominator;
        }
    }
}
