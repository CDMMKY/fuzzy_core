using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace TestJavaInputParams
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> iparams = new Dictionary<string, string>();

            string [] tempValue = null;
            for (int i = 0; i < args.Count(); i++)
            {  string test = "";
          
                tempValue = args[i].Split( new Char [] {'=','/'},StringSplitOptions.RemoveEmptyEntries);


//                tempValue = tempValue.Where(x => ! x.Equals("",S));
                for (int k = 1; k < tempValue.Count(); k++)
                {
                    test += tempValue[k]; 
                }

                    iparams.Add(tempValue[0], test);
            }

            int j=0;
            foreach (string key in iparams.Keys)
            {

               Console.WriteLine("Get params №{0}, named {1} and have value {2}" +Environment.NewLine,j.ToString(), key, iparams[key] );
                j++;
            }

            string[] filesName = Directory.GetFiles(iparams["SourceDir"], "*.ufs");
            int countto = System.Convert.ToInt32(iparams["toCount"]);
            int currentTo =0;
            int iterate = System.Convert.ToInt32(iparams["RunIter"]);
            foreach (string fileName in filesName)
            {
                if (currentTo >= countto) break;
                FileInfo fi = new FileInfo(fileName);
                File.Copy(fileName, iparams["DestinyDir"] + fi.Name);
                currentTo++;

                System.Threading.Thread.Sleep(10 * iterate);
            }




           


        }
    }
}
