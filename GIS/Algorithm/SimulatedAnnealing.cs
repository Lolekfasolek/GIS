using GIS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIS.Algorithm
{
    /// <summary>
    /// Class implementing simulated annealing for chinese postmaster.
    /// </summary>
    public class SimulatedAnnealing
    {
        private Random random = new Random();

        /// <summary>
        /// Main method to find solution.
        /// </summary>
        /// <param name="graph">Graph to find solution within.</param>
        /// <param name="startVertex">Start vertex.</param>
        /// <param name="initialTemperature">Inital temperature.</param>
        /// <param name="finalTemperature">Final Temperature.</param>
        /// <param name="numberOfInnerLoops">Number of how many times inner loop should be done for each temperature.</param>
        /// <param name="coolingAlpha">Alpha for cooling.</param>
        /// <returns>Best found solution.</returns>
        public List<Edge> FindSolution(Graph graph, Vertex startVertex, double initialTemperature, double finalTemperature, int numberOfInnerLoops, double coolingAlpha)
        {
            if (!graph.Vertices.Contains(startVertex))
            {
                throw new InvalidOperationException("SIMULATED ANNEALING: Start vertex is not in graph!");
            }
            var actualSolution = FindFirstSolution(graph, startVertex);
            Console.WriteLine("Initial solution cost: " + actualSolution.GetSolutionCost());
            var bestSolution = actualSolution;
            var temperature = initialTemperature;
            do
            {
                for (var i = 0; i < numberOfInnerLoops; i++)
                {
                    var neighbour = GenerateNeighbour(actualSolution, graph);
                    Console.WriteLine("Generated neighbour cost: " + neighbour.GetSolutionCost());
                    if (neighbour.GetSolutionCost() < actualSolution.GetSolutionCost()
                        || (this.random.NextDouble() <= Math.Exp(-Math.Abs(neighbour.GetSolutionCost() - actualSolution.GetSolutionCost()) / temperature)))
                    {
                        actualSolution = neighbour;
                    }
                    if (actualSolution.GetSolutionCost() < bestSolution.GetSolutionCost())
                    {
                        bestSolution = actualSolution;
                    }
                }
                Console.WriteLine("Actual cost: " + actualSolution.GetSolutionCost());
                temperature = Cooling(temperature, coolingAlpha);
            } while (temperature > finalTemperature);
            return bestSolution;
        }

        /// <summary>
        /// Method for cooling temperature.
        /// </summary>
        /// <param name="temperature">Actual temperature.</param>
        /// <param name="coolingAlpha">Colling alpha.</param>
        /// <returns>Cooled temperature.</returns>
        private double Cooling(double temperature, double coolingAlpha)
        {
            return temperature * coolingAlpha;
        }

        /// <summary>
        /// Method used for finding first solution. It uses modified random walking.
        /// </summary>
        /// <param name="graph">Graph to generate initail solution for.</param>
        /// <param name="startVertex">Start vertex.</param>
        /// <returns>Generated solution.</returns>
        private List<Edge> FindFirstSolution(Graph graph, Vertex startVertex)
        {
            var edges = graph.Edges;
            var actual = startVertex;
            var usedEdges = new HashSet<Edge>();
            var result = new List<Edge>();
            do
            {
                var notUsedEdges = actual.GetNotUsedEdges(usedEdges);
                if (notUsedEdges.Count > 0)
                {
                    var notUsedEdge = notUsedEdges[(int)Math.Round(this.random.NextDouble() * (notUsedEdges.Count - 1))];
                    edges.Remove(notUsedEdge);
                    result.Add(notUsedEdge);
                    actual = notUsedEdge.GetOtherVertex(actual);
                }
                else
                {
                    actual = this.GoToClosestVertexWithNotUsedEdge(actual, usedEdges, result);
                }
            } while (edges.Count != 0);
            this.GoToVertex(actual, startVertex, result);
            return result;
        }

        /// <summary>
        /// Finds a way to closest vertex (in number of edges) which has any not used edge.
        /// </summary>
        /// <param name="actual">Actual vertex.</param>
        /// <param name="usedEdges">Used edges so far - to determine whether vertex has used edges or not.</param>
        /// <param name="result">Result path to add edges while going to edge.</param>
        /// <returns>Found edge.</returns>
        private Vertex GoToClosestVertexWithNotUsedEdge(Vertex actual, HashSet<Edge> usedEdges, List<Edge> result)
        {
            var queue = new Queue<Vertex>();
            var usedVertices = new HashSet<Vertex>();
            queue.Enqueue(actual);
            Vertex found = null;
            while(queue.Count > 0){
                var vertex = queue.Dequeue();
                usedVertices.Add(vertex);
                if (vertex.HasNotUsedEdge(usedEdges))
                {
                    found = vertex;
                    break;
                }
                foreach (var edge in vertex.Edges)
                {
                    var otherVertex = edge.GetOtherVertex(vertex);
                    if (!usedVertices.Contains(otherVertex))
                    {
                        usedVertices.Add(otherVertex);
                        queue.Enqueue(otherVertex);
                    }
                }
            }
            this.GoToVertex(actual, found, result);
            return found;
        }

        /// <summary>
        /// Moves solution to given vertex.
        /// </summary>
        /// <param name="start">Start vertex.</param>
        /// <param name="destination">Destination vertex.</param>
        /// <param name="result">Result path to add edges while going to destination.</param>
        private void GoToVertex(Vertex start, Vertex destination, List<Edge> result)
        {
            if (start == destination)
            {
                return;
            }
            var queue = new Queue<Vertex>();
            var usedVertices = new HashSet<Vertex>();
            var parent = new Dictionary<Vertex, Vertex>();
            queue.Enqueue(start);
            var found = false;
            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                usedVertices.Add(vertex);
                if (vertex == destination)
                {
                    found = true;
                    break;
                }
                foreach (var edge in vertex.Edges)
                {
                    var otherVertex = edge.GetOtherVertex(vertex);
                    if (!usedVertices.Contains(otherVertex))
                    {
                        usedVertices.Add(otherVertex);
                        queue.Enqueue(otherVertex);
                        parent.Add(otherVertex, vertex);
                    }
                }
            }
            if (!found)
            {
                throw new InvalidOperationException("SIMULATED ANNEALING: Didn't found route from start edge to destination");
            }
            var actualVertex = destination;
            var revertedList = new List<Edge>();
            while (actualVertex != start)
            {
                var previousVertex = parent[actualVertex];
                var edge = this.FindEdgeBetweenVertices(actualVertex, previousVertex);
                revertedList.Add(edge);
                actualVertex = previousVertex;
            }
            revertedList.Reverse();
            result.AddRange(revertedList);
        }

        /// <summary>
        /// Finds edge between given verices.
        /// </summary>
        /// <param name="firstVertex">First vertex.</param>
        /// <param name="secondVertex">Second vertex.</param>
        /// <returns>Edge between those vertices.</returns>
        private Edge FindEdgeBetweenVertices(Vertex firstVertex, Vertex secondVertex)
        {
            var edges = firstVertex.Edges.Where(e => secondVertex.Edges.Contains(e));
            if (edges.Count() > 1)
            {
                throw new InvalidOperationException("SIMULATED ANNEALING: There are multiple edges between two vertices!");
            }
            if (edges.Count() == 0)
            {
                throw new InvalidOperationException("SIMULATED ANNEALING: There is no edge between two vertices!");
            }
            return edges.First();
        }

        /// <summary>
        /// Finds vertex between given edges.
        /// </summary>
        /// <param name="firstEdge">First edge.</param>
        /// <param name="secondEdge">Second edge.</param>
        /// <returns>Vertex between those edges.</returns>
        private Vertex FindVertexBetweenEdges(Edge firstEdge, Edge secondEdge)
        {
            if (firstEdge.FirstVertex == secondEdge.FirstVertex || firstEdge.FirstVertex == secondEdge.SecondVertex)
            {
                return firstEdge.FirstVertex;
            }
            else if (firstEdge.SecondVertex == secondEdge.FirstVertex || firstEdge.SecondVertex == secondEdge.SecondVertex)
            {
                return firstEdge.SecondVertex;
            }
            throw new InvalidOperationException("SIMULATED ANNEALING: There is no common vertex between edges!");
        }

        /// <summary>
        /// Generates neighbour. It removes random edge from solution and tries to add shortest path within it.
        /// </summary>
        /// <param name="actualSolution">Actual solution.</param>
        /// <param name="graph">Graph.</param>
        /// <returns>Neighbour solution.</returns>
        private List<Edge> GenerateNeighbour(List<Edge> actualSolution, Graph graph)
        {
            var edgeNumber = (int)Math.Round(this.random.NextDouble() * (actualSolution.Count - 1));
            var edge = actualSolution[edgeNumber];
            var leftSolution = actualSolution.Take(edgeNumber).ToList();
            var rightSolution = actualSolution.Skip(edgeNumber + 1).ToList();
            if (leftSolution.Count + rightSolution.Count + 1 != actualSolution.Count)
            {
                throw new InvalidOperationException("SIMULATED ANNEALING: Number of solutions after split should be same as before split!");
            }
            Vertex leftVertex;
            Vertex rightVertex;
            if (leftSolution.Count > 0)
            {
                var lastEdge = leftSolution.Last();
                leftVertex = this.FindVertexBetweenEdges(lastEdge, edge);
                rightVertex = edge.GetOtherVertex(leftVertex);
            }
            else if (rightSolution.Count > 0)
            {
                var firstEdge = rightSolution.First();
                rightVertex = this.FindVertexBetweenEdges(firstEdge, edge);
                leftVertex = edge.GetOtherVertex(rightVertex);
            }
            else
            {
                throw new InvalidOperationException("SIMULATED ANNEALING: Left and right solutions have 0 elements!");
            }
            var shortestPath = this.ShortestPath(leftVertex, rightVertex, graph);
            if (!(leftSolution.Contains(edge) || rightSolution.Contains(edge) || shortestPath.Contains(edge)))
            {
                shortestPath.Add(edge);
                shortestPath.Add(edge);
            }
            leftSolution.AddRange(shortestPath);
            leftSolution.AddRange(rightSolution);
            return leftSolution;
        }

        /// <summary>
        /// Finds shortest path between given vertices using dijkstra algorithm.
        /// </summary>
        /// <param name="from">Start vertex.</param>
        /// <param name="to">Destination vertex.</param>
        /// <param name="graph">Graph.</param>
        /// <returns>List of edges to go from start to destination vertices.</returns>
        private List<Edge> ShortestPath(Vertex from, Vertex to, Graph graph)
        {
            var distance = new Dictionary<Vertex, double>();
            var predescor = new Dictionary<Vertex, Vertex>();
            foreach (var vertex in graph.Vertices)
            {
                distance.Add(vertex, double.MaxValue);
            }
            distance[from] = 0;
            var queue = graph.Vertices;
            while (queue.Count > 0)
            {
                var orderedQueue = queue.OrderBy(v => distance[v]);
                var vertex = orderedQueue.First();
                queue.Remove(vertex);
                if (vertex == to)
                {
                    break;
                }
                foreach (var neighbour in vertex.Edges.Select(e => e.GetOtherVertex(vertex)))
                {
                    if (distance[neighbour] > distance[vertex] + this.FindEdgeBetweenVertices(vertex, neighbour).Length)
                    {
                        distance[neighbour] = distance[vertex] + this.FindEdgeBetweenVertices(vertex, neighbour).Length;
                        if (predescor.ContainsKey(neighbour))
                        {
                            predescor[neighbour] = vertex;
                        }
                        else
                        {
                            predescor.Add(neighbour, vertex);
                        }
                    }
                }
            }

            var actualVertex = to;
            var reversedList = new List<Edge>();
            while (actualVertex != from)
            {
                var previousVertex = predescor[actualVertex];
                reversedList.Add(this.FindEdgeBetweenVertices(actualVertex, previousVertex));
                actualVertex = previousVertex;
            }
            reversedList.Reverse();
            return reversedList;
        }
    }
}
