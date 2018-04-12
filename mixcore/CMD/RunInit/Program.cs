using System.Reflection;
using System.IO;
namespace RunInit
{
    class Program
    {
       
       
        static int Main(string[] args)
        {
            FileInfo main_file = new FileInfo(Assembly.GetExecutingAssembly().Location);
            switch (main_file.Directory.Name)
            {
                case "35":
                    {
                        Wrapper35 init_alg = new Wrapper35();
                        return init_alg.Run(args);

                    }
                case "25":
                    {
                        Wrapper25 init_alg = new Wrapper25();
                        return init_alg.Run(args);

                    }
                case "36":
                    {
                        Wrapper36_script init_alg = new Wrapper36_script();
                        return init_alg.Run(args);

                    }


                case "40":
                    {
                        Wrapper40 init_alg = new Wrapper40();
                        return init_alg.Run(args);

                    }

                case "41":
                    {
                        Wrapper41 init_alg = new Wrapper41();
                        return init_alg.Run(args);

                    }

            /*    case "42":
                    {
                        Wrapper42 init_alg = new Wrapper42();
                        return init_alg.Run(args);


                    }
                    */


                case "99":
                    {
                        Wrapper99 init_alg = new Wrapper99();
                        return init_alg.Run(args);

                    }
                case "999":
                    {
                        Wrapper999 init_alg = new Wrapper999();
                        return init_alg.Run(args);

                    }


                default:
                    {
                        Wrapper36_script init_alg = new Wrapper36_script();
                        return init_alg.Run(args);
                    }

                  
            }
          
         
        }
    }
}
