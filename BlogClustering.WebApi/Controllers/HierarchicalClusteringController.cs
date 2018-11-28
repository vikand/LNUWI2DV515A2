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
    public class HierarchicalClusteringController : ControllerBase
    {
        private readonly IBlogDataRepository blogDataRepository;

        public HierarchicalClusteringController(IBlogDataRepository blogDataRepository)
        {
            this.blogDataRepository = blogDataRepository;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<Cluster> Get(bool useCache)
        {
            var blogs = blogDataRepository.GetBlogData();
            var pearson = new PearsonCorrelationSimilarityAlgorithm();
            var hierarchicalClustering = new HierarchicalClustering(pearson, useCache);

            return hierarchicalClustering.Cluster(blogs);
        }
    }
}
