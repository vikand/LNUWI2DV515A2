using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlogClustering.Entities
{
    public class Blog
    {
        public string Name { get; set; }

        //[JsonIgnore]
        public Dictionary<string, double> Words { get; set; }
    }
}
