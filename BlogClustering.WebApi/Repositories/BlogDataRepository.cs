using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogClustering.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BlogClustering.WebApi.Repositories
{
    public class BlogDataRepository: IBlogDataRepository
    {
        private IConfiguration configuration;
        private IHostingEnvironment hostingEnvironment;

        public BlogDataRepository(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            this.configuration = configuration;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IEnumerable<Blog> GetBlogData()
        {
            var blogs = new List<Blog>();
            var lines = File.ReadAllLines(GetFilePath());
            var words = lines[0].Split('\t'); ;

            for (var lineIndex = 1; lineIndex < lines.Length; lineIndex++)
            {
                var counts = lines[lineIndex].Split('\t');
                var blog = new Blog { Name = counts[0], Words = new Dictionary<string, double>() };

                for (var wordIndex = 1; wordIndex < words.Length; wordIndex++)
                {
                    blog.Words.Add(words[wordIndex], int.Parse(counts[wordIndex]));
                }

                blogs.Add(blog);
            }

            return blogs;
        }

        private string GetFilePath()
        {
            var rootPath = this.hostingEnvironment.ContentRootPath;
            var relativePathToBlogDataFile = this.configuration["RelativePathToBlogDataFile"];

            return Path.Combine(rootPath, relativePathToBlogDataFile);
        }
    }
}
