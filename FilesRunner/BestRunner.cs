using GIS.Algorithm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesRunner
{
    public class BestRunner
    {
        private double firstBest;
        private double secondBest;
        private double thirdBest;
        private double firstWorst;
        private double secondWorst;
        private double thirdWorst;

        private Dictionary<double, List<string>> valueWithFiles;

        public void ProcessAll(string rootDirectory)
        {
            var directories = Directory.GetDirectories(rootDirectory);
            foreach (var directory in directories)
            {
                var files = Directory.GetFiles(directory, "*_iter?.txt").OrderBy(s => s).ToArray();
                var directoryName = Path.GetFileName(directory);
                Console.WriteLine("Directory: " + directoryName);
                var fileLogger = new FileLogger(rootDirectory + Path.DirectorySeparatorChar + directoryName + "_best.txt");
                valueWithFiles = new Dictionary<double, List<string>>();
                this.InitBestAndWorst();
                this.ProcessFiles(files);
                fileLogger.LogLine("Best value: " + firstBest.ToString());
                valueWithFiles[firstBest].ForEach(s => fileLogger.LogLine(s));
                fileLogger.LogLine("");
                fileLogger.LogLine("----------------------------------------------------------------");
                fileLogger.LogLine("");
                fileLogger.LogLine("Second best value: " + secondBest.ToString());
                valueWithFiles[secondBest].ForEach(s => fileLogger.LogLine(s));
                fileLogger.LogLine("");
                fileLogger.LogLine("----------------------------------------------------------------");
                fileLogger.LogLine("");
                fileLogger.LogLine("Third best value: " + thirdBest.ToString());
                valueWithFiles[thirdBest].ForEach(s => fileLogger.LogLine(s));

                fileLogger.LogLine("");
                fileLogger.LogLine("----------------------------------------------------------------");
                fileLogger.LogLine("");
                fileLogger.LogLine("Third worst value: " + thirdWorst.ToString());
                valueWithFiles[thirdWorst].ForEach(s => fileLogger.LogLine(s));
                fileLogger.LogLine("");
                fileLogger.LogLine("----------------------------------------------------------------");
                fileLogger.LogLine("");
                fileLogger.LogLine("Second worst value: " + secondWorst.ToString());
                valueWithFiles[secondWorst].ForEach(s => fileLogger.LogLine(s));
                fileLogger.LogLine("");
                fileLogger.LogLine("----------------------------------------------------------------");
                fileLogger.LogLine("");
                fileLogger.LogLine("Worst value: " + firstWorst.ToString());
                valueWithFiles[firstWorst].ForEach(s => fileLogger.LogLine(s));
                fileLogger.Dispose();
            }
        }

        private void ProcessFiles(string[] files)
        {
            foreach (var file in files)
            {
                var value = this.GetFileSolutionCost(file);
                this.UpdateBest(value);
                this.UpdateWorst(value);
                if (!valueWithFiles.ContainsKey(value))
                {
                    valueWithFiles.Add(value, new List<string>());
                }
                valueWithFiles[value].Add(Path.GetFileName(file));
            }
        }

        private double GetFileSolutionCost(string file)
        {
            var lines = File.ReadAllLines(file);
            var line = lines.First(s => s.StartsWith("Final solution cost"));
            return double.Parse(line.Replace("Final solution cost: ", ""));
        }

        private void InitBestAndWorst()
        {
            this.firstBest = double.MaxValue;
            this.secondBest = double.MaxValue;
            this.thirdBest = double.MaxValue;
            this.firstWorst = double.MinValue;
            this.secondWorst = double.MinValue;
            this.thirdWorst = double.MinValue;
        }

        private void UpdateBest(double value)
        {
            if (value < firstBest)
            {
                thirdBest = secondBest;
                secondBest = firstBest;
                firstBest = value;
            }
            else if (firstBest == value)
            {
                return;
            }
            else if (value < secondBest)
            {
                thirdBest = secondBest;
                secondBest = value;
            }
            else if (secondBest == value)
            {
                return;
            }
            else if (value < thirdBest)
            {
                thirdBest = value;
            }
        }

        private void UpdateWorst(double value)
        {
            if (value > firstWorst)
            {
                thirdWorst = secondWorst;
                secondWorst = firstWorst;
                firstWorst = value;
            }
            else if (value == firstWorst)
            {
                return;
            }
            else if (value > secondWorst)
            {
                thirdWorst = secondWorst;
                secondWorst = value;
            }
            else if (value == secondWorst)
            {
                return;
            }
            else if (value > thirdWorst)
            {
                thirdWorst = value;
            }
        }
    }
}
