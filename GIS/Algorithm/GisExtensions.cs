using GIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIS.Algorithm
{
    /// <summary>
    /// Extensions methods used in algorithm.
    /// </summary>
    internal static class GisExtensions
    {
        /// <summary>
        /// Gets the cost of solution.
        /// </summary>
        /// <param name="solution">Solution to get cost for.</param>
        /// <returns>Cost of this solution (sum of cost f all edges)</returns>
        public static double GetSolutionCost(this List<Edge> solution)
        {
            return solution.Aggregate(0.0, (actual, edge) => actual + edge.Length);
        }

        /// <summary>
        /// Returns not used edges in this vertex.
        /// </summary>
        /// <param name="vertex">Vertex to get all not used edges.</param>
        /// <param name="usedEdges">Set with used edges.</param>
        /// <returns>List with not used edges connected to given vertex.</returns>
        public static List<Edge> GetNotUsedEdges(this Vertex vertex, HashSet<Edge> usedEdges)
        {
            return vertex.Edges.Where(e => !usedEdges.Contains(e)).ToList();
        }

        /// <summary>
        /// Returns true if vertex has not used edge.
        /// </summary>
        /// <param name="vertex">Vertex to check.</param>
        /// <param name="usedEdges">Set with used edges.</param>
        /// <returns>True if vertex has not used edges.</returns>
        public static bool HasNotUsedEdge(this Vertex vertex, HashSet<Edge> usedEdges)
        {
            return vertex.GetNotUsedEdges(usedEdges).Count > 0;
        }
    }
}
