using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIS.Algorithm
{
    public class FileLogger : IDisposable
    {
        private string fileName;

        private List<string> lines = new List<string>();

        public FileLogger(string fileName)
        {
            this.fileName = fileName;
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        public void LogLine(string line)
        {
            this.lines.Add(line);
            if (this.lines.Count > 500)
            {
                this.WriteLines();
            }
        }

        private void WriteLines()
        {
            File.AppendAllLines(this.fileName, this.lines);
            this.lines.Clear();
        }

        public void Dispose()
        {
            this.WriteLines();
        }
    }
}
