using System;
using BlogClustering.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogClustering.WebApp.Pages
{
    public class HierarchicalClusteringModel : PageModel
    {
        private IHttpClientWrapper client;

        public HierarchicalClusteringModel(IHttpClientWrapper client)
        {
            this.client = client;
        }

        [BindProperty]
        public bool UseCache { get; set; }

        public TimeSpan Duration { get; set; }

        public Cluster Cluster { get; set; }

        public void OnPost()
        {
            var startTime = DateTime.Now;

            Cluster = client.Get<Cluster>($"api/hierarchicalclustering?usecache={UseCache}").Item1;

            Duration = DateTime.Now - startTime;
        }
    }
}
