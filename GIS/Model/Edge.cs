using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIS.Model
{
    /// <summary>
    /// Class representing edge in graph.
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// Lenght of this edge.
        /// </summary>
        public double Length
        {
            get;
            private set;
        }

        /// <summary>
        /// First vertex connected to this edge.
        /// </summary>
        public Vertex FirstVertex
        {
            get;
            private set;
        }

        /// <summary>
        /// Second vertex connected to this edge.
        /// </summary>
        public Vertex SecondVertex
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates new Edge.
        /// </summary>
        /// <param name="length">Length of edge</param>
        /// <param name="firstVertex">First vertex of edge.</param>
        /// <param name="secondVertex">Second edge of vertex.</param>
        internal Edge(double length, Vertex firstVertex, Vertex secondVertex)
        {
            this.Length = length;
            this.FirstVertex = firstVertex;
            this.SecondVertex = secondVertex;
        }

        /// <summary>
        /// Returns the other vertex than given one.
        /// It means if given vertex is first vertex, then second vertex is returned.
        /// It throws exception if given vertex is not first or second vertex.
        /// </summary>
        /// <param name="vertex">Vertex to return other one.</param>
        /// <returns>Other vertex than given one.</returns>
        public Vertex GetOtherVertex(Vertex vertex)
        {
            if (this.FirstVertex == vertex)
            {
                return this.SecondVertex;
            }
            if (this.SecondVertex == vertex)
            {
                return this.FirstVertex;
            }
            throw new InvalidOperationException("EDGE: Given vertex is not connected to this edge!");
        }
    }
}
