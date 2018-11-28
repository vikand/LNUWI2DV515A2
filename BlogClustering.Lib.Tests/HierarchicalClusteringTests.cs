using BlogClustering.Entities;
using BlogClustering.Lib.Algorithms;
using System.Collections.Generic;
using Xunit;

namespace BlogClustering.Lib.Tests
{
    public class HierarchicalClusteringTests
    {
        [Fact]
        public void ClusterWithZeroBlogsGivesZeroClusters()
        {
            // Arrange

            var sut = new HierarchicalClustering(new PearsonCorrelationSimilarityAlgorithm(), false);

            // Act

            var result = sut.Cluster(new List<Blog>());

            // Assert

            Assert.Null(result);
        }

        [Fact]
        public void ClusterWithOneBlogsGivesOneCluster()
        {
            // Arrange

            var blogs = new List<Blog>
            {
                new Blog
                {
                    Name = "Blog1",
                    Words = new Dictionary<string, double>
                    {
                        { "word1", 5 },
                        { "word2", 0 },
                        { "word3", 5 },
                    },
                },
            };

            var sut = new HierarchicalClustering(new PearsonCorrelationSimilarityAlgorithm(), false);

            // Act

            var result = sut.Cluster(blogs);

            // Assert

            Assert.Equal("Blog1", result.Blog.Name);
            Assert.Null(result.Left);
            Assert.Null(result.Right);
        }

        [Fact]
        public void ClusterWith3BlogsGives3Clusters()
        {
            // Arrange

            var blogs = new List<Blog>
            {
                new Blog
                {
                    Name = "Blog1",
                    Words = new Dictionary<string, double>
                    {
                        { "word1", 5 },
                        { "word2", 0 },
                        { "word3", 5 },
                    },
                },
                new Blog
                {
                    Name = "Blog2",
                    Words = new Dictionary<string, double>
                    {
                        { "word1", 0 },
                        { "word2", 3 },
                        { "word3", 0 },
                    },
                },
                new Blog
                {
                    Name = "Blog3",
                    Words = new Dictionary<string, double>
                    {
                        { "word1", 5 },
                        { "word2", 0 },
                        { "word3", 5 },
                    },
                },
            };

            var sut = new HierarchicalClustering(new PearsonCorrelationSimilarityAlgorithm(), false);

            // Act

            var result = sut.Cluster(blogs);

            // Assert

            Assert.Equal("Blog1", result.Left.Left.Blog.Name);
            Assert.Equal("Blog3", result.Left.Right.Blog.Name);
            Assert.Equal("Blog2", result.Right.Blog.Name);
        }
    }
}
