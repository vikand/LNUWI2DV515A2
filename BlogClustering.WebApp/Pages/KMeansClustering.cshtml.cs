using System;
using System.Collections.Generic;
using BlogClustering.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogClustering.WebApp.Pages
{
    public class KMeansClusteringModel : PageModel
    {
        private IHttpClientWrapper client;

        public KMeansClusteringModel(IHttpClientWrapper client)
        {
            this.client = client;
            NumberOfClusters = 5;
            NumberOfIterations = -1;
        }

        [BindProperty]
        public int NumberOfClusters { get; set; }

        [BindProperty]
        public int NumberOfIterations { get; set; }

        public TimeSpan Duration { get; set; }

        public IEnumerable<Centroid> Cluster { get; set; }

        public void OnPost()
        {
            var startTime = DateTime.Now;

            Cluster = client.Get<IEnumerable<Centroid>>(
                $"api/kmeansclustering?numberofclusters={NumberOfClusters}&numberofiterations={NumberOfIterations}").Item1;

            Duration = DateTime.Now - startTime;
        }
    }
}
