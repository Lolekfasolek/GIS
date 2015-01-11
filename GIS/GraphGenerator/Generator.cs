using GIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIS.GraphGenerator
{
    public class Generator
    {
        public const double MAX_COST = 100;

        public static Graph GenerateNewGraph(int numberOfVertices)
        {
            Graph resultGraph;
            var alpha = 0.5;
            var iteration = 0;
            var random = new Random();
            do
            {
                if (iteration > 10)
                {
                    iteration = 0;
                    alpha += 0.05;
                }
                resultGraph = new Graph();
                for (int i = 0; i < numberOfVertices; i++)
                {
                    resultGraph.AddVertex(i);
                }

                foreach (var firstVertex in resultGraph.Vertices)
                {
                    foreach (var secondVertex in resultGraph.Vertices)
                    {
                        if (firstVertex != secondVertex && !IsPairUsed(firstVertex, secondVertex, resultGraph))
                        {
                            if (random.NextDouble() <= alpha)
                            {
                                var cost = (random.NextDouble() * (MAX_COST - 1)) + 1;
                                resultGraph.AddEdge(cost, firstVertex, secondVertex);
                            }
                        }
                    }
                }
                iteration++;
            } while (!CheckGraphIntegrity(resultGraph));
            Console.WriteLine(string.Format("Created graph with {0} vertices with {1} alpha and {2} iterations", numberOfVertices, alpha, iteration));
            return resultGraph;
        }

        private static bool IsPairUsed(Vertex firstVertex, Vertex secondVertex, Graph resultGraph)
        {
            return resultGraph.Edges.Any(e => (e.FirstVertex == firstVertex && e.SecondVertex == secondVertex) || (e.FirstVertex == secondVertex && e.SecondVertex == firstVertex));
        }

        /// <summary>
        /// Checks graph integrity. Returns true if graph is consistent.
        /// </summary>
        /// <param name="graph">Graph to check its integrity.</param>
        /// <returns>True if graph is integral (consistent)</returns>
        private static bool CheckGraphIntegrity(Graph graph)
        {
            var visited = new Dictionary<Vertex, bool>();
            var stack = new Stack<Vertex>();
            var visitedCount = 0;
            var fistVertex = graph.Vertices.First();
            stack.Push(fistVertex);
            visited.Add(fistVertex, true);
            // DEPTH FIRST SEARCH
            while (stack.Count != 0)
            {
                var actual = stack.Pop();
                visitedCount++;
                foreach (var neighbour in actual.Edges.Select(e => e.GetOtherVertex(actual)))
                {
                    if (!(visited.ContainsKey(neighbour) && visited[neighbour]))
                    {
                        visited.Add(neighbour, true);
                        stack.Push(neighbour);
                    }
                }
            }
            return visitedCount == graph.Vertices.Count;
        }
    }
}
