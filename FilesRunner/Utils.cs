using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesRunner
{
    public class Utils
    {
        public static int GetTemperature(string fileName)
        {
            var value = fileName.Replace("text_size", "");
            value = RemoveNumbers(value);
            value = value.Replace("_temp", "");
            return GetAsInt(value);
        }

        public static int GetInnerLoops(string fileName)
        {
            var value = fileName.Replace("text_size", "");
            value = RemoveNumbers(value);
            value = value.Replace("_temp", "");
            value = RemoveNumbers(value);
            value = value.Replace("_inner", "");
            var result = "";
            do
            {
                var actChar = value.First();
                result += actChar;
                value = value.Remove(0, 1);

            } while (value.Length > 0);
            return int.Parse(result);
        }

        public static int GetGraphSize(string fileName)
        {
            var value = fileName.Replace("text_size", "");
            return GetAsInt(value);
        }

        private static int GetAsInt(string value)
        {
            var result = "";
            var tmp = value;
            var actChar = tmp.First();
            while (actChar != '_')
            {
                result += actChar;
                tmp = tmp.Remove(0, 1);
                actChar = tmp.First();
            }
            return int.Parse(result);
        }

        private static string RemoveNumbers(string s)
        {
            var tmp = s;
            var actChar = tmp.First();
            while (actChar != '_')
            {
                tmp = tmp.Remove(0, 1);
                actChar = tmp.First();
            }
            return tmp;
        }
    }
}
