using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlogClustering.Entities
{
    public class Centroid
    {
        //[JsonIgnore]
        public Dictionary<string, double> Words { get; set; }

        public List<Blog> Blogs { get; set; }
    }
}
