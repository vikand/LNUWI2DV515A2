namespace BlogClustering.Entities
{
    public class Cluster
    {
        public Cluster Left { get; set; }
        public Cluster Right { get; set; }
        public Blog Blog { get; set; }
        public double Distance { get; set; }
    }
}
