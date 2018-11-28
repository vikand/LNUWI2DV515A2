using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogClustering.Entities;
using BlogClustering.Lib.Algorithms;
using BlogClustering.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BlogClustering.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KMeansClusteringController : ControllerBase
    {
        private readonly IBlogDataRepository blogDataRepository;

        public KMeansClusteringController(IBlogDataRepository blogDataRepository)
        {
            this.blogDataRepository = blogDataRepository;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Centroid>> Get(int numberOfClusters, int numberOfIterations)
        {
            var blogs = blogDataRepository.GetBlogData();
            var pearson = new PearsonCorrelationSimilarityAlgorithm();
            var kMeansClustering = new KMeansClustering(pearson);

            return kMeansClustering.Cluster(blogs, numberOfClusters, numberOfIterations).ToArray();
        }
    }
}
