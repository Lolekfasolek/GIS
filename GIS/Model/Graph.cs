using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIS.Model
{
    /// <summary>
    /// Class representing graph.
    /// </summary>
    public class Graph
    {
        private List<Edge> edges = new List<Edge>();
        private List<Vertex> vertices = new List<Vertex>();

        /// <summary>
        /// Returns list of edges in graph.
        /// </summary>
        public List<Edge> Edges
        {
            get
            {
                return this.edges.ToList();
            }
        }

        /// <summary>
        /// Returns list of verices in graph.
        /// </summary>
        public List<Vertex> Vertices
        {
            get
            {
                return this.vertices.ToList();
            }
        }

        /// <summary>
        /// Adds new vertex to graph.
        /// </summary>
        /// <param name="id">
        /// Id of vertex.
        /// </param>
        /// <returns>Added vertex.</returns>
        public Vertex AddVertex(int id)
        {
            var vertex = new Vertex(id);
            this.vertices.Add(vertex);
            return vertex;
        }

        public Edge AddEdge(double length, Vertex firstVertex, Vertex secondVertex)
        {
            var edge = new Edge(length, firstVertex, secondVertex);
            this.edges.Add(edge);
            firstVertex.AddEdge(edge);
            secondVertex.AddEdge(edge);
            return edge;
        }

    }
}
