using BlogClustering.Entities;
using BlogClustering.Lib.Algorithms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace BlogClustering.Lib.Tests
{
    public class JsonFileFixture
    {
        public JsonFileFixture()
        {
            var pathToJsonFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data\\blogs.json");
            var json = File.ReadAllText(pathToJsonFile);
            var settings = new JsonSerializerSettings { MetadataPropertyHandling = MetadataPropertyHandling.Ignore };

            Blogs = JsonConvert.DeserializeObject<IList<Blog>>(json, settings);
        }

        public IEnumerable<Blog> Blogs { get; }
    }

    public class KMeansClusteringTests : IClassFixture<JsonFileFixture>
    {
        private readonly IEnumerable<Blog> blogs;

        public KMeansClusteringTests(JsonFileFixture fixture)
        {
            this.blogs = fixture.Blogs;
        }

        // N.B. For the deserialization to work ok, comment out JsonIgnore attributes!!!

        [Fact]
        public void ClusterWith99BlogsGoogleBlogsShouldAlwayBeClusteredTogether()
        {
            ClusterWith99BlogsSomeBlogsShouldAlwayBeClusteredTogether(new[]
            {
                "Google Operating System",
                "Google Blogoscoped"
            });
        }

        [Fact]
        public void ClusterWith99BlogsSearchEngineBlogsShouldAlwayBeClusteredTogether()
        {
            ClusterWith99BlogsSomeBlogsShouldAlwayBeClusteredTogether(new[]
            {
                "Search Engine Watch Blog",
                "John Battelle's Searchblog",
                "Search Engine Roundtable"
            });
        }

        private void ClusterWith99BlogsSomeBlogsShouldAlwayBeClusteredTogether(string[] blogsThatShouldBeClusteredTogether)
        {
            // Arrange

            var sut = new KMeansClustering(new PearsonCorrelationSimilarityAlgorithm());

            for (var iteration = 0; iteration <= 10; iteration++)
            {
                // Act

                var result = sut.Cluster(this.blogs, 5, -1).ToArray();

                // Assert

                Assert.Equal(5, result.Count());

                Centroid centroidWithBlogsThatShouldBeClusteredTogether = null;

                foreach (var centroid in result)
                {
                    if (centroid.Blogs.Any(b => b.Name == blogsThatShouldBeClusteredTogether.First()))
                    {
                        centroidWithBlogsThatShouldBeClusteredTogether = centroid;
                        break;
                    }
                }

                foreach (var blogName in blogsThatShouldBeClusteredTogether)
                {
                    Assert.Contains(centroidWithBlogsThatShouldBeClusteredTogether.Blogs, b => b.Name == blogName);
                }
            }
        }
    }
}
