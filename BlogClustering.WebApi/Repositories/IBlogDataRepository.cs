using System.Collections.Generic;
using BlogClustering.Entities;

namespace BlogClustering.WebApi.Repositories
{
    public interface IBlogDataRepository
    {
        IEnumerable<Blog> GetBlogData();
    }
}
