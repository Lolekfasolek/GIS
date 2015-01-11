using GIS.Algorithm;
using GIS.GraphGenerator;
using GIS.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIS
{
    class Program
    {
        private static List<int> innerLoops = new List<int>
        {
            1,2,3,4,5,6,7,8,9,10,15,20
        };

        private static List<int> temperatures = new List<int>
        {
            50, 100, 150, 200, 300, 400, 500, 750, 1000, 1500, 2000
        };

        static void Main(string[] args)
        {
            var algorithm = new SimulatedAnnealing();
            Console.WriteLine("Started");
            for (var graphSize = 5; graphSize <= 25; graphSize += 5)
            {
                var graph = Generator.GenerateNewGraph(graphSize);
                SaveGraphToFile(graph, "graph" + graphSize + ".txt");
                for (var temperature = 0; temperature < temperatures.Count; temperature++)
                {
                    for (var inner = 0; inner < innerLoops.Count; inner++)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            var file = "text";
                            file += "_size" + graphSize.ToString();
                            file += "_temp" + temperatures[temperature].ToString();
                            file += "_inner" + innerLoops[inner].ToString();
                            file += "_iter" + i.ToString();
                            file += ".txt";
                            var solution = algorithm.FindSolution(graph, graph.Vertices.First(), temperatures[temperature], 1, innerLoops[inner], 0.92, file);
                            Console.WriteLine("DONE: Graph size: " + graphSize + ". Temperature: " + temperatures[temperature] + ". Inner loops: " + innerLoops[inner] + ". Iteration: " + i);
                        }
                    }
                }
            }

            //var graph = Generator.GenerateNewGraph(10);
            //Console.WriteLine("Found solution cost: " + solution.GetSolutionCost());
            Console.WriteLine("Finished");
            Console.ReadLine();
        }

        private static void SaveGraphToFile(Graph graph, string file)
        {
            var lines = new List<string>();
            var line = "\t";
            foreach (var vertex in graph.Vertices.OrderBy(v => v.Id))
            {
                line += vertex.Id + "\t";
            }
            lines.Add(line);
            foreach (var vertex in graph.Vertices.OrderBy(v => v.Id))
            {
                line = "";
                line += vertex.Id + "\t";
                foreach (var otherVertex in graph.Vertices.OrderBy(v => v.Id))
                {
                    if (vertex == otherVertex)
                    {
                        line += "0\t";
                    }
                    else
                    {
                        var edge = vertex.Edges.Where(e => otherVertex.Edges.Contains(e)).FirstOrDefault();
                        if (edge == null)
                        {
                            line += "0\t";
                        }
                        else
                        {
                            line += edge.Length.ToString() + "\t";
                        }
                    }
                }
                lines.Add(line);
            }
            File.WriteAllLines(file, lines);
        }
    }
}
