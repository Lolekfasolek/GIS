using GIS.Algorithm;
using GIS.GraphGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIS
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph = Generator.GenerateNewGraph(10);
            var algorithm = new SimulatedAnnealing();
            Console.WriteLine("Started");
            var solution = algorithm.FindSolution(graph, graph.Vertices.First(), 2000, 1, 20, 0.95);
            Console.WriteLine("Found solution cost: " + solution.GetSolutionCost());
            Console.WriteLine("Finished");
            Console.ReadLine();
        }
    }
}
