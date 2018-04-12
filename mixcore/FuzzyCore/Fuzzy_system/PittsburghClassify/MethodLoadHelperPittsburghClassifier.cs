using FuzzySystem.FuzzyAbstract;

using FuzzySystem.PittsburghClassifier.LearnAlgorithm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


namespace FuzzySystem.PittsburghClassifier
{
  public  class MethodLoadHelperPittsburghClassifier
    {
        object lockGen = new object();
        object lockLearn = new object();
        public List<AbstractNotSafeGenerator> InstanceOfInit { get { return instanceOfInit; } }
        List<AbstractNotSafeGenerator> instanceOfInit = new List<AbstractNotSafeGenerator>();
        string MethodOfInitPath = (new FileInfo(Assembly.GetExecutingAssembly().Location)).DirectoryName + "\\Methods\\Init";
        List<Type> AlreadyAddedInit = new List<Type>();

        public List<IAbstractLearnAlgorithm> InstanceOfTune { get { return instanceOfTune; } }
        List<IAbstractLearnAlgorithm> instanceOfTune = new List<IAbstractLearnAlgorithm>();
        string MethodsOfTunePath = (new FileInfo(Assembly.GetExecutingAssembly().Location)).DirectoryName + "\\Methods\\Tune";
        List<Type> AlreadyAddedTune = new List<Type>();
        Type Init = typeof(IAbstractGenerator);
        Type Tune = typeof(IAbstractLearnAlgorithm);

        public MethodLoadHelperPittsburghClassifier()
        {
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
                        if (!AlreadyAddedInit.Contains(sourceType))
                        {
                            instanceOfInit.Add(method.CreateInstance(sourceType.FullName) as AbstractNotSafeGenerator);
                            AlreadyAddedInit.Add(sourceType);
                        }
                    }
                }

                if (sourceType.GetInterfaces().Contains(Tune))
                {
                    lock (lockLearn)
                    {
                        if (!AlreadyAddedTune.Contains(sourceType))
                        {
                            instanceOfTune.Add(method.CreateInstance(sourceType.FullName) as IAbstractLearnAlgorithm);
                            AlreadyAddedTune.Add(sourceType);
                        }
                    }
                }
            }
        }



    }
}
