using GIS.Algorithm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesRunner
{
    public class Program
    {
        static void Main(string[] args)
        {
            //var bestRunner = new BestRunner();
            //bestRunner.ProcessAll(@"E:\studia\mgr 3 sem\GIS\Wyniki2");

            var boringRunner = new BoringRunner();
            boringRunner.ProcessAll(@"E:\studia\mgr 3 sem\GIS\Wyniki2");
        }
    }
}
