
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FuzzySystem.FuzzyAbstract.Hybride;

namespace FuzzySystem.FuzzyAbstract
{
  public  class BaseMethodLoad
    {


      FuzzySystemRelisedList.TypeSystem TypeFuzzySystem;
        object lockGen = new object();
        object lockLearn = new object();
        public List<IAbstractGenerator> InstanceOfInit { get { return instanceOfInit; } }
        List<IAbstractGenerator> instanceOfInit = new List<IAbstractGenerator>();
        string MethodOfInitPath = (new FileInfo(Assembly.GetExecutingAssembly().Location)).DirectoryName + "\\Methods\\Init";
        List <string>  AlreadyAddedInit = new List<string >();

        public List<IAbstractLearnAlgorithm> InstanceOfTune { get { return instanceOfTune; } }
        List<IAbstractLearnAlgorithm> instanceOfTune = new List<IAbstractLearnAlgorithm>();
        string MethodsOfTunePath = (new FileInfo(Assembly.GetExecutingAssembly().Location)).DirectoryName + "\\Methods\\Tune";
        List<string> AlreadyAddedTune = new List<string>();
        Type Init = typeof(IAbstractGenerator);
        Type Tune = typeof(IAbstractLearnAlgorithm);
        public BaseMethodLoad(FuzzySystemRelisedList.TypeSystem TypeFS)
        {
            TypeFuzzySystem = TypeFS;
            string[] fileinit = Directory.GetFiles(MethodOfInitPath, "*.dll");
            string[] fileTune = Directory.GetFiles(MethodsOfTunePath, "*.dll");
            Parallel.ForEach(fileinit, tryLoadMethod);
            Parallel.ForEach(fileTune, tryLoadMethod);
            CompararerForMethods forSort = new CompararerForMethods();

            instanceOfInit.Distinct();
            instanceOfInit.Sort(forSort);

            instanceOfTune.Distinct();
            instanceOfTune.Sort(forSort);
            GC.Collect();

        }

        private void tryLoadMethod(string file)
        {
            try
            {
                Assembly method = Assembly.LoadFile(file);
                Type[] test = method.GetTypes();
                Parallel.ForEach(test, (type) => { tryloadinstance(type, method); });

            }
            catch (ReflectionTypeLoadException e)
            {
                if (e.LoaderExceptions != null)
                {
                    foreach (Exception ex in e.LoaderExceptions)
                    {
                        Console.WriteLine(ex.Message);

                    }
                    return;
                }
               
                Console.WriteLine(e.Message);
       
            }

        }

        private void tryloadinstance(Type sourceType, Assembly method)
        {
            if (sourceType.IsClass)
            {              
                if (sourceType.GetInterfaces().Contains(Init))
                {
                    lock (lockGen)
                    {
                        if (!AlreadyAddedInit.Contains(sourceType.FullName))
                        {
                            IAbstractGenerator TempAG = method.CreateInstance(sourceType.FullName) as IAbstractGenerator;
                            if (TempAG.SupportedFS.Contains(TypeFuzzySystem))
                            { 
                            instanceOfInit.Add(TempAG);
                            AlreadyAddedInit.Add(sourceType.FullName);
                            }
                        }
                    }
                }

                if (sourceType.GetInterfaces().Contains(Tune))
                {
                    lock (lockLearn)
                    {
                        if (!AlreadyAddedTune.Contains(sourceType.FullName))
                        {
                            IAbstractLearnAlgorithm tempALA = method.CreateInstance(sourceType.FullName) as IAbstractLearnAlgorithm;
                            if (tempALA.SupportedFS.Contains(TypeFuzzySystem))
                            { 
                            instanceOfTune.Add(tempALA);
                            AlreadyAddedTune.Add(sourceType.FullName);
                            }
                        }
                    }
                }
            }
        }



    
}


    
}
