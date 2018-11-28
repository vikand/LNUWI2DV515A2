using System;
using System.Collections.Generic;
using System.Linq;
using BlogClustering.Entities;
using BlogClustering.Lib.Extenders;

namespace BlogClustering.Lib.Algorithms
{
    public class KMeansClustering 
    {
        private readonly ISimilarityAlgorithm similarityAlgorithm;

        public KMeansClustering(ISimilarityAlgorithm similarityAlgorithm)
        {
            this.similarityAlgorithm = similarityAlgorithm;
        }

        public IEnumerable<Centroid> Cluster(IEnumerable<Blog> blogs, int numberOfClusters, int numberOfIterations)
        {
            var allWords = new Dictionary<string, Tuple<int,int>>();
            
            var words = blogs
                .SelectMany(b => b.Words)
                .GroupBy(w => w.Key)
                .Select(g => new { Name = g.Key, Min = g.Min(m => m.Value), Max = g.Max(m => m.Value) });

            var currentCentroids = new Centroid[numberOfClusters];
            var random = new Random();

            for (var index = 0; index < numberOfClusters; index++)
            {
                var centroid = new Centroid
                {
                    Words = new Dictionary<string, double>(),
                    Blogs = new List<Blog>(),
                };

                foreach (var word in words)
                {
                    centroid.Words.Add(word.Name, random.Next((int)word.Min, (int)word.Max));
                }

                currentCentroids[index] = centroid;
            }

            var lastCentroids = new Centroid[numberOfClusters];
            var iterations = 0;

            do
            {
                for (var index = 0; index < numberOfClusters; index++)
                {
                    lastCentroids[index] = currentCentroids[index];
                    currentCentroids[index] = new Centroid
                    {
                        Words = lastCentroids[index].Words.ToDictionary(w => w.Key, w => w.Value),
                        Blogs = new List<Blog>(),
                    };
                }

                foreach (var blog in blogs)
                {
                    var shortestDistance = double.MaxValue;
                    Centroid bestCentroid = null;

                    foreach (var centroid in currentCentroids)
                    {
                        var distance = CalculateDistance(blog, centroid);

                        if (distance < shortestDistance)
                        {
                            bestCentroid = centroid;
                            shortestDistance = distance;
                        }
                    }

                    bestCentroid.Blogs.Add(blog);
                }

                foreach (var centroid in currentCentroids)
                {
                    foreach (var key in centroid.Words.Keys.ToArray())
                    {
                        var wordCount = 0d;
                        var blogCount = 0d;

                        foreach (var blog in centroid.Blogs)
                        {
                            if (blog.Words.ContainsKey(key))
                            {
                                wordCount = blog.Words[key];
                                blogCount++;
                            }
                        }

                        centroid.Words[key] = wordCount / blogCount;
                    }
                }

                iterations++;

            } while ((numberOfIterations > 0 && iterations < numberOfIterations) ||
                     (numberOfIterations <= 0 && 
                      !currentCentroids.IsEqualTo(lastCentroids, ObjectComparisionMethod.Json)));

            return currentCentroids;
        }

        private double CalculateDistance(Blog blog, Centroid centeroid)
        {
            var pearson = new PearsonCorrelationSimilarityAlgorithm();
            var similarity = pearson.CalculateSimilarity(blog.Words, centeroid.Words);

            return 1 - similarity;
        }
    }
}
