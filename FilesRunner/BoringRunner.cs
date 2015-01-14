using GIS.Algorithm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesRunner
{
    public class BoringRunner
    {
        private Dictionary<string, List<int>> oneChangeBoredom;
        private Dictionary<string, List<int>> twoChangeBoredom;
        private Dictionary<string, List<int>> threeChangeBoredom;
        private Dictionary<string, List<int>> fourChangeBoredom;
        private Dictionary<string, int> numberOfIterations;

        public void ProcessAll(string rootDirectory)
        {
            var directories = Directory.GetDirectories(rootDirectory);
            foreach (var directory in directories)
            {
                var files = Directory.GetFiles(directory, "*_iter?.txt").OrderBy(s => s).ToArray();
                var directoryName = Path.GetFileName(directory);
                Console.WriteLine("Directory: " + directoryName);
                var fileLogger = new FileLogger(rootDirectory + Path.DirectorySeparatorChar + directoryName + "_boredom_one_change_reverse_Loops.txt");
                this.oneChangeBoredom = new Dictionary<string, List<int>>();
                this.twoChangeBoredom = new Dictionary<string, List<int>>();
                this.threeChangeBoredom = new Dictionary<string, List<int>>();
                this.fourChangeBoredom = new Dictionary<string, List<int>>();
                this.numberOfIterations = new Dictionary<string, int>();
                this.ProcessFiles(files);

                var temperatures = oneChangeBoredom.Keys.Select(k => Utils.GetTemperature(k)).OrderBy(i => i).Distinct();
                var innerLoops = oneChangeBoredom.Keys.Select(k => Utils.GetInnerLoops(k)).OrderByDescending(i => i).Distinct();

                fileLogger.LogLine(temperatures.Aggregate("", (s, t) => s + "\t" + t.ToString()));

                var graphSize = Utils.GetGraphSize(oneChangeBoredom.Keys.First());
                foreach (var innerLoop in innerLoops)
                {
                    var line = innerLoop.ToString();
                    foreach (var temperature in temperatures)
                    {
                        var key = oneChangeBoredom.Keys.First(k => k == "text_size" + graphSize.ToString() + "_temp" + temperature.ToString() + "_inner" + innerLoop.ToString());
                        var value = Averange(oneChangeBoredom[key]);
                        line += "\t" + value;
                    }
                    fileLogger.LogLine(line);
                }

                //fileLogger.LogLine("\\Numer iteracji\t1\t2\t3\t4\t5\t6\t7\t8\t9\t10\t\tLiczba iteracji");

                //foreach (var key in oneChangeBoredom.Keys.OrderBy(s=>s, new FileNameComparer()))
                //{
                //    //fileLogger.LogLine(key);
                //    // TODO order by temperature, then inner
                //    fileLogger.LogLine(key + " :" + oneChangeBoredom[key].Aggregate("", (s, i) => s + "\t" + i.ToString()) + "\t\t" + numberOfIterations[key]);
                //    //fileLogger.LogLine("Second :" + twoChangeBoredom[key].Aggregate("", (s, i) => s + "\t" + i.ToString()) + "\t\t" + numberOfIterations[key]);
                //    //fileLogger.LogLine("Third :" + threeChangeBoredom[key].Aggregate("", (s, i) => s + "\t" + i.ToString()) + "\t\t" + numberOfIterations[key]);
                //    //fileLogger.LogLine("Fourth :" + fourChangeBoredom[key].Aggregate("", (s, i) => s + "\t" + i.ToString()) + "\t\t" + numberOfIterations[key]);
                //}

                fileLogger.Dispose();
            }
        }

        private class FileNameComparer: IComparer<string>
        {

            public int Compare(string x, string y)
            {
                var xs = x as string;
                var ys = y as string;
                var xt = Utils.GetTemperature(xs);
                var yt = Utils.GetTemperature(ys);
                if (xt != yt)
                {
                    return xt > yt ? 1 : -1;
                }

                var lx = Utils.GetInnerLoops(xs);
                var ly = Utils.GetInnerLoops(ys);
                return lx == ly ? 0 : (lx > ly ? 1 : -1);
            }
        }

        private void ProcessFiles(string[] files)
        {
            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file).Skip(2).ToArray();
                lines = lines.Take(lines.Count() - 2).ToArray();
                var fileName = Path.GetFileNameWithoutExtension(file);
                fileName = fileName.Remove(fileName.Length - 6);
                this.AddOneChangeBoredom(lines, fileName);
                this.AddTwoChangeBoredom(lines, fileName);
                this.AddThreeChangeBoredom(lines, fileName);
                this.AddFourChangeBoredom(lines, fileName);

                if (!this.numberOfIterations.ContainsKey(fileName))
                {
                    this.numberOfIterations.Add(fileName, lines.Length);
                }
            }
        }

        private void AddOneChangeBoredom(string[] lines, string fileName)
        {
            var reversedLines = lines.Reverse();
            var firstValue = reversedLines.First();
            var counter = 0;
            foreach (var line in reversedLines)
            {
                if (line == firstValue)
                {
                    counter++;
                }
                else
                {
                    break;
                }
            }
            counter--;
            if (!this.oneChangeBoredom.ContainsKey(fileName))
            {
                this.oneChangeBoredom.Add(fileName, new List<int>());
            }
            this.oneChangeBoredom[fileName].Add(counter);
        }

        private void AddTwoChangeBoredom(string[] lines, string fileName)
        {
            var reversedLines = lines.Reverse();
            var firstValue = reversedLines.First();
            string secondValue = null;
            var counter = 0;
            foreach (var line in reversedLines)
            {
                if (line == firstValue || line == secondValue)
                {
                    counter++;
                }
                else if (secondValue == null)
                {
                    secondValue = line;
                    counter++;
                }
                else
                {
                    break;
                }
            }
            if (!this.twoChangeBoredom.ContainsKey(fileName))
            {
                this.twoChangeBoredom.Add(fileName, new List<int>());
            }
            this.twoChangeBoredom[fileName].Add(counter);
        }

        private void AddThreeChangeBoredom(string[] lines, string fileName)
        {
            var reversedLines = lines.Reverse();
            var firstValue = reversedLines.First();
            string secondValue = null;
            string thirdValue = null;
            var counter = 0;
            foreach (var line in reversedLines)
            {
                if (line == firstValue || line == secondValue || line == thirdValue)
                {
                    counter++;
                }
                else if (secondValue == null)
                {
                    secondValue = line;
                    counter++;
                }
                else if (thirdValue == null)
                {
                    thirdValue = line;
                    counter++;
                }
                else
                {
                    break;
                }
            }
            if (!this.threeChangeBoredom.ContainsKey(fileName))
            {
                this.threeChangeBoredom.Add(fileName, new List<int>());
            }
            this.threeChangeBoredom[fileName].Add(counter);
        }

        private void AddFourChangeBoredom(string[] lines, string fileName)
        {
            var reversedLines = lines.Reverse();
            var firstValue = reversedLines.First();
            string secondValue = null;
            string thirdValue = null;
            string fourthValue = null;
            var counter = 0;
            foreach (var line in reversedLines)
            {
                if (line == firstValue || line == secondValue || line == thirdValue || line == fourthValue)
                {
                    counter++;
                }
                else if (secondValue == null)
                {
                    secondValue = line;
                    counter++;
                }
                else if (thirdValue == null)
                {
                    thirdValue = line;
                    counter++;
                }
                else if (fourthValue == null)
                {
                    fourthValue = line;
                    counter++;
                }
                else
                {
                    break;
                }
            }
            if (!this.fourChangeBoredom.ContainsKey(fileName))
            {
                this.fourChangeBoredom.Add(fileName, new List<int>());
            }
            this.fourChangeBoredom[fileName].Add(counter);
        }

        private double Averange(List<int> list)
        {
            return list.Aggregate(0, (i, e) => i + e) / ((double)list.Count);
        }
    }
}
