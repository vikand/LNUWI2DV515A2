using System;
using System.Collections.Generic;
using System.Linq;
using BlogClustering.Entities;

namespace BlogClustering.Lib.Algorithms
{
    public class HierarchicalClustering
    {
        private readonly ISimilarityAlgorithm similarityAlgorithm;
        private readonly Dictionary<string, double> cache;

        public HierarchicalClustering(ISimilarityAlgorithm similarityAlgorithm, bool useCache)
        {
            this.similarityAlgorithm = similarityAlgorithm;
            cache = useCache ? new Dictionary<string, double>() : null;
        }

        public Cluster Cluster(IEnumerable<Blog> blogs)
        {
            var clusters = blogs
                .Select(b => new Cluster { Blog = b })
                .ToList();

            while (clusters.Count > 1)
            {
                Cluster closestClusterA = null;
                Cluster closestClusterB = null;
                var shortestDistance = Double.MaxValue;

                foreach (var clusterA in clusters)
                {
                    foreach (var clusterB in clusters)
                    {
                        if (clusterA != clusterB)
                        {
                            var distance = CalculateDistance(clusterA.Blog, clusterB.Blog);

                            if (distance < shortestDistance)
                            {
                                shortestDistance = distance;
                                closestClusterA = clusterA;
                                closestClusterB = clusterB;
                            }
                        }
                    }
                }

                if (closestClusterA != null)
                {
                    MergeClusters(clusters, closestClusterA, closestClusterB, shortestDistance);
                }
            }

            return clusters.FirstOrDefault();
        }

        private double CalculateDistance(Blog blogA, Blog blogB)
        {
            if (this.cache != null && blogA.Name != null && blogB.Name != null)
            {
                var key = blogA.Name + "+" + blogB.Name;

                if (this.cache.ContainsKey(key))
                {
                    return this.cache[key];
                }

                var distance = 1 - this.similarityAlgorithm.CalculateSimilarity(blogA.Words, blogB.Words);

                this.cache.Add(key, distance);

                return distance;
            }

            return 1 - this.similarityAlgorithm.CalculateSimilarity(blogA.Words, blogB.Words);
        }

        private void MergeClusters(List<Cluster> clusters, Cluster clusterA, Cluster clusterB, double distance)
        {
            clusters.Remove(clusterA);
            clusters.Remove(clusterB);

            var parentCluster = new Cluster
            {
                Left = clusterA,
                Right = clusterB,
                Distance = distance,
                Blog = new Blog { Words = new Dictionary<string, double>() },
            };

            foreach (var wordA in clusterA.Blog.Words)
            {
                var countB = clusterB.Blog.Words[wordA.Key];

                parentCluster.Blog.Words.Add(wordA.Key, (wordA.Value + countB) / 2);
            }

            clusters.Insert(0, parentCluster);
        }
    }
}
