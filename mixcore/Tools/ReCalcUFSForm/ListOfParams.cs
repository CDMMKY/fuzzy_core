using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReCalcUFSForm
{
    public class ListOfParams
    {
        protected int completedFile = 0;
        protected int completedColums = 0;

        public int CompletedColums
        {
            get { return completedColums; }

        }

        public int CompletedFile
        {
            get { return completedFile; }
        }
        List<string> nameFile = new List<string>();
        List<string> NameFile
        {
            get { return nameFile; }
        }


        List<double> GIBNormalI = new List<double>();

        public List<double> GIBNormal
        {
            get { return GIBNormalI; }
        }
        List<double> GIBSumStraigthI = new List<double>();

        public List<double> GIBSumStraigth
        {
            get { return GIBSumStraigthI; }
        }

        List<double> GIBSumReverceI = new List<double>();

        public List<double> GIBSumReverce
        {
            get { return GIBSumReverceI; }
        }

        List<double> GISNormalI = new List<double>();

        public List<double> GISNormal
        {
            get { return GISNormalI; }

        }

        List<double> GISSumStraigthI = new List<double>();

        public List<double> GISSumStraigth
        {
            get { return GISSumStraigthI; }

        }



        List<double> GISSumReverceI = new List<double>();

        public List<double> GISSumReverce
        {
            get { return GISSumReverceI; }

        }

        List<double> GICNormalI = new List<double>();

        public List<double> GICNormal
        {
            get { return GICNormalI; }
        }

        List<double> GICSumStraighI = new List<double>();

        public List<double> GICSumStraigh
        {
            get { return GICSumStraighI; }
        }

        List<double> GICSumReverceI = new List<double>();

        public List<double> GICSumReverce
        {
            get { return GICSumReverceI; }
        }

        List<double> LindisNormalI = new List<double>();

        public List<double> LindisNormal
        {
            get { return LindisNormalI; }

        }


        List<double> LindisSumStraighI = new List<double>();

        public List<double> LindisSumStraigh
        {
            get { return LindisSumStraighI; }
        }

        List<double> LindisSumReverceI = new List<double>();

        public List<double> LindisSumReverce
        {
            get { return LindisSumReverceI; }
        }
        List<double> NormalIndexI = new List<double>();

        public List<double> NormalIndex
        {
            get { return NormalIndexI; }
        }
        List<double> SumsStraigthIndexI = new List<double>();

        public List<double> SumsStraigthIndex
        {
            get { return SumsStraigthIndexI; }
        }
        List<double> SumReverseIndexI = new List<double>();

        public List<double> SumReverseIndex
        {
            get { return SumReverseIndexI; }
        }

        XLSAndTextFilesTools.XLSWriter tempWriter;


        List<RecombineUFSExample> UFSLoadedData = new List<RecombineUFSExample>();


        public ListOfParams()
        { }

        public void loadData(List<string> UFSData)
        {

            Parallel.ForEach(UFSData, x =>
            {
                RecombineUFSExample tempSolution = new RecombineUFSExample(x);
                tempSolution.Work();
                lock (UFSLoadedData)
                {
                    UFSLoadedData.Add(tempSolution);
                }

                completedFile++;
            });

        }
        public ListOfParams(List<string> UFSData)
        {
            loadData(UFSData);
            Init();
        }

        public void Init()
        {
            completedFile = 0;
            foreach (RecombineUFSExample x in UFSLoadedData)
            {
                GIBNormalI.Add(x.GIBNormal);
                GIBSumStraigthI.Add(x.GIBSumStraigth);
                GIBSumReverceI.Add(x.GIBSumReverce);

                GICNormalI.Add(x.GICNormal);
                GICSumReverceI.Add(x.GICSumReverce);
                GICSumStraighI.Add(x.GICSumStraigh);

                GISNormalI.Add(x.GISNormal);
                GISSumReverceI.Add(x.GISSumReverce);
                GISSumStraigthI.Add(x.GISSumStraigth);

                LindisNormalI.Add(x.LindisNormal);
                LindisSumReverceI.Add(x.LindisSumReverce);
                LindisSumStraigh.Add(x.LindisSumStraigh);

                NormalIndexI.Add(x.NormalIndex);
                SumReverseIndexI.Add(x.SumReverseIndex);
                SumsStraigthIndexI.Add(x.SumsStraigthIndex);

                nameFile.Add(x.SourceI);
                completedFile++;

            }
        }
        public ListOfParams(List<RecombineUFSExample> Source)
        {
            UFSLoadedData = Source;
            Init();
        }

        public Task savetoXLS(string path, string name)
        {
            Task Global = new Task(() =>
            {
                completedColums = 0;
                using (tempWriter = new XLSAndTextFilesTools.XLSWriter(path, name, 4))
                {

                    const int startCell = 1;
                    Task[] Tasks = new Task[4];
                    Tasks[0] = (new Task(() =>
                    {
                        string liter1 = "A";

                        tempWriter.writeOneColumn(liter1, startCell, "Имя файла", nameFile.ToArray(), 1);
                        completedColums++;
                        liter1 = XLSAndTextFilesTools.XLSWriter.inc_literal(liter1);

                        tempWriter.writeOneColumn(liter1, startCell, "GIBNormal", GIBNormal.ToArray(), 1);
                        completedColums++;
                        liter1 = XLSAndTextFilesTools.XLSWriter.inc_literal(liter1);

                        tempWriter.writeOneColumn(liter1, startCell, "GICNormal", GICNormal.ToArray(), 1);
                        completedColums++;
                        liter1 = XLSAndTextFilesTools.XLSWriter.inc_literal(liter1);

                        tempWriter.writeOneColumn(liter1, startCell, "GISNormal", GISNormal.ToArray(), 1);
                        completedColums++;
                        liter1 = XLSAndTextFilesTools.XLSWriter.inc_literal(liter1);

                        tempWriter.writeOneColumn(liter1, startCell, "LindisNormal", LindisNormal.ToArray(), 1);
                        completedColums++;
                        liter1 = XLSAndTextFilesTools.XLSWriter.inc_literal(liter1);

                        tempWriter.writeOneColumn(liter1, startCell, "NormalIndex", NormalIndex.ToArray(), 1);
                        completedColums++;



                    }));
                    Tasks[0].Start();

                    Tasks[1] = (new Task(() =>
                                  {
                                      string liter2 = "A";

                                      tempWriter.writeOneColumn(liter2, startCell, "Имя файла", nameFile.ToArray(), 2);
                                      completedColums++;
                                      liter2 = XLSAndTextFilesTools.XLSWriter.inc_literal(liter2);


                                      tempWriter.writeOneColumn(liter2, startCell, "GIBSumStraigth", GIBSumStraigth.ToArray(), 2);
                                      completedColums++;
                                      liter2 = XLSAndTextFilesTools.XLSWriter.inc_literal(liter2);

                                      tempWriter.writeOneColumn(liter2, startCell, "GICSumStraigth", GICSumStraigh.ToArray(), 2);
                                      completedColums++;
                                      liter2 = XLSAndTextFilesTools.XLSWriter.inc_literal(liter2);

                                      tempWriter.writeOneColumn(liter2, startCell, "GISSumStraigth", GISSumStraigth.ToArray(), 2);
                                      completedColums++;
                                      liter2 = XLSAndTextFilesTools.XLSWriter.inc_literal(liter2);

                                      tempWriter.writeOneColumn(liter2, startCell, "LindisSumStraigth", LindisSumStraigh.ToArray(), 2);
                                      completedColums++;
                                      liter2 = XLSAndTextFilesTools.XLSWriter.inc_literal(liter2);

                                      tempWriter.writeOneColumn(liter2, startCell, "SumsStraigthIndex", SumsStraigthIndex.ToArray(), 2);
                                      completedColums++;


                                  }));
                    Tasks[1].Start();


                    Tasks[2] = (new Task(() =>
                                  {
                                      string liter3 = "A";

                                      tempWriter.writeOneColumn(liter3, startCell, "Имя файла", nameFile.ToArray(), 3);
                                      completedColums++;
                                      liter3 = XLSAndTextFilesTools.XLSWriter.inc_literal(liter3);

                                      tempWriter.writeOneColumn(liter3, startCell, "GIBSumReverce", GIBSumReverce.ToArray(), 3);
                                      completedColums++;
                                      liter3 = XLSAndTextFilesTools.XLSWriter.inc_literal(liter3);

                                      tempWriter.writeOneColumn(liter3, startCell, "GICSumReverce", GICSumReverce.ToArray(), 3);
                                      completedColums++;
                                      liter3 = XLSAndTextFilesTools.XLSWriter.inc_literal(liter3);

                                      tempWriter.writeOneColumn(liter3, startCell, "GISSumReverce", GISSumReverce.ToArray(), 3);
                                      completedColums++;
                                      liter3 = XLSAndTextFilesTools.XLSWriter.inc_literal(liter3);

                                      tempWriter.writeOneColumn(liter3, startCell, "LindisSumReverce", LindisSumReverce.ToArray(), 3);
                                      completedColums++;
                                      liter3 = XLSAndTextFilesTools.XLSWriter.inc_literal(liter3);

                                      tempWriter.writeOneColumn(liter3, startCell, "SumReverseIndex", SumReverseIndex.ToArray(), 3);
                                      completedColums++;

                                  }));
                    Tasks[2].Start();


                    Task.WaitAll(Tasks.ToArray());


                    tempWriter.Save();
                }
            });

            return Global;

        }


        public Task savetoTXT(string path, string name1, string name2, string name3)
        {
            Task Global = new Task(() =>
            {

                completedFile = 0;
                Task[] Tasks = new Task[6];
                Tasks[0] = new Task(
                    () =>
                    {
                        using (System.IO.StreamWriter sw1 = new System.IO.StreamWriter(System.IO.Path.Combine(path, name1), false))
                        {
                            sw1.WriteLine("GIBNormal\tGICNormal\tGISNormal\tLindisNormal\tNormalIndex");
                            for (int i = 0; i < nameFile.Count; i++)
                            {
                                sw1.WriteLine(GIBNormal[i].ToString() + "\t" + GICNormal[i].ToString() + "\t" + GISNormal[i].ToString() + "\t" + LindisNormal[i].ToString() + "\t" + NormalIndex[i].ToString());
                                completedFile++;

                            }
                        }
                    });

                Tasks[0].Start();

                Tasks[1] = new Task(
                () =>
                {
                    using (System.IO.StreamWriter sw1 = new System.IO.StreamWriter(System.IO.Path.Combine(path, name1 + "_test.txt"), false))
                    {
                        sw1.WriteLine("GIBNormal\tGICNormal\tGISNormal\tLindisNormal");
                        for (int i = 0; i < nameFile.Count; i++)
                        {
                            sw1.WriteLine(GIBNormal[i].ToString() + "\t" + GICNormal[i].ToString() + "\t" + GISNormal[i].ToString() + "\t" + LindisNormal[i].ToString());
                            completedFile++;

                        }
                    }
                });

                Tasks[1].Start();

                Tasks[2] = new Task(
              () =>
              {
                  using (System.IO.StreamWriter sw2 = new System.IO.StreamWriter(System.IO.Path.Combine(path, name2), false))
                  {
                      sw2.WriteLine("GIBSumStraigth\tGICSumStraigth\tGISSumStraigth\tLindisSumStraigth\tSumStraigthIndex");
                      for (int i = 0; i < nameFile.Count; i++)
                      {
                          sw2.WriteLine(GIBSumStraigth[i].ToString() + "\t" + GICSumStraigh[i].ToString() + "\t" + GISSumStraigth[i].ToString() + "\t" + LindisSumStraigh[i].ToString() + "\t" + SumsStraigthIndex[i].ToString());
                          completedFile++;

                      }
                  }
              });
                Tasks[2].Start();

                Tasks[3] = new Task(
         () =>
         {
             using (System.IO.StreamWriter sw2 = new System.IO.StreamWriter(System.IO.Path.Combine(path, name2 + "_test.txt"), false))
             {
                 sw2.WriteLine("GIBSumStraigth\tGICSumStraigth\tGISSumStraigth\tLindisSumStraigth");
                 for (int i = 0; i < nameFile.Count; i++)
                 {
                     sw2.WriteLine(GIBSumStraigth[i].ToString() + "\t" + GICSumStraigh[i].ToString() + "\t" + GISSumStraigth[i].ToString() + "\t" + LindisSumStraigh[i].ToString());
                     completedFile++;

                 }
             }
         });
                Tasks[3].Start();



                Tasks[4] = new Task(
          () =>
          {
              using (System.IO.StreamWriter sw3 = new System.IO.StreamWriter(System.IO.Path.Combine(path, name3), false))
              {
                  sw3.WriteLine("GIBSumReverce\tGICSumReverce\tGISSumReverce\tLindisSumReverce\tSumReverceIndex");

                  for (int i = 0; i < nameFile.Count; i++)
                  {
                      sw3.WriteLine(GIBSumReverce[i].ToString() + "\t" + GICSumReverce[i].ToString() + "\t" + GISSumReverce[i].ToString() + "\t" + LindisSumReverce[i].ToString() + "\t" + SumReverseIndex[i].ToString());

                      completedFile++;

                  }
              }
          });
                Tasks[4].Start();

                Tasks[5] = new Task(
    () =>
    {
        using (System.IO.StreamWriter sw3 = new System.IO.StreamWriter(System.IO.Path.Combine(path, name3 + "_test.txt"), false))
        {
            sw3.WriteLine("GIBSumReverce\tGICSumReverce\tGISSumReverce\tLindisSumReverce");

            for (int i = 0; i < nameFile.Count; i++)
            {
                sw3.WriteLine(GIBSumReverce[i].ToString() + "\t" + GICSumReverce[i].ToString() + "\t" + GISSumReverce[i].ToString() + "\t" + LindisSumReverce[i].ToString());

                completedFile++;

            }
        }
    });

                Tasks[5].Start();

                Task.WaitAll(Tasks);

            });
            return Global;
        }

    }

}