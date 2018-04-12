using System.Collections.Generic;
using System.Linq;

namespace RunInit
{
    abstract class Base
    {
        protected string file_out;

        protected abstract void fill_params(string[] args);
        protected abstract void fill_conf();
        public abstract int Run(string[] args);


        public string listtoOne(List<string> toArgv)
        {
            string result = "";
            for (int i = 0; i < toArgv.Count - 1; i++)
            {
                result += toArgv[i] + "}";
            }
            result += toArgv[toArgv.Count - 1];
            return result;
        }


        protected string confParams = "";

        public void toStringParams(string[] argv)
        {
            List<string> toargv = argv.ToList();
            if (toargv.Where(x => x.Contains("in")).FirstOrDefault() != null) toargv.Remove((argv.Where(x => x.Contains("in"))).ToArray()[0]);
            if (toargv.Where(x => x.Contains("out")).FirstOrDefault() != null) toargv.Remove((argv.Where(x => x.Contains("out"))).ToArray()[0]);
            if (toargv.Where(x => x.Contains("Path")).FirstOrDefault() != null) toargv.Remove((argv.Where(x => x.Contains("Path"))).ToArray()[0]);
            confParams = listtoOne(toargv);

        }

    }
}
