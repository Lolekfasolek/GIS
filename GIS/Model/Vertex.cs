using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIS.Model
{
    public class Vertex
    {
        private List<Edge> edges = new List<Edge>();

        /// <summary>
        /// Gets list of edges connected to this vertex.
        /// </summary>
        public List<Edge> Edges
        {
            get
            {
                return this.edges.ToList();
            }
        }

        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates new vertex.
        /// </summary>
        /// <param name="id">
        /// Id of vertex.
        /// </param>
        internal Vertex(int id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Adds edge to this vertex.
        /// </summary>
        /// <param name="edge">Edge to add to this vertex.</param>
        internal void AddEdge(Edge edge)
        {
            if (this != edge.FirstVertex && this != edge.SecondVertex)
            {
                throw new InvalidOperationException("EDGE: trying to connect edge to vertex that shouldnt be connected to it!");
            }
            this.edges.Add(edge);
        }
    }
}
