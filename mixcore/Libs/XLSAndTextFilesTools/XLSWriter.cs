using System;
using System.Linq;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace XLSAndTextFilesTools
{
    public class XLSWriter:IDisposable
    {

           Excel.Application exapp = new Excel.Application();
           Excel.Workbook exworkbook;
        

        string FullPath;
        public XLSWriter(string path, string nameFile, int Count)
        {
            FullPath = Path.Combine(path, nameFile);
            exapp = new Excel.Application();
              exapp.Workbooks.Add();
                exworkbook = exapp.Workbooks[1];
                while (exapp.Sheets.Count < Count)
                {
                    exapp.Sheets.Add();
                }
        }

        public void Save()
        { if (File.Exists(FullPath))
            {
                File.Delete(FullPath);
            }
        exworkbook.SaveAs(FullPath);
        }
   
       
            public void writeOneColumn( string Literal, int CountDest, string NameColumn, Array Source, int NumberofSheet=1)
            { 
                Excel.Worksheet sheet = exapp.Sheets[NumberofSheet];
              
           

            Excel.Range temp = sheet.get_Range(Literal + (CountDest ).ToString());
            temp.Value2 = NameColumn;

                for (int i = 1; i <= Source.Length; i++)
                {
                    temp = sheet.get_Range(Literal + (CountDest + i).ToString());
                    temp.Value2 = Source.GetValue(i-1);

                }
            }

            static public string inc_literal(string source)
            {
                string result;

                char[] source_char = source.ToArray();
                if (source_char[source_char.Count() - 1] == 'Z')
                {

                    source_char[source_char.Count() - 1] = 'A';

                    if ((source_char.Count()) - 1 > 0)
                    {
                        source_char[source_char.Count() - 2] = (char)((int)(source_char[source_char.Count() - 2]) + 1);
                    }
                    else
                    {
                        string temp = "A";

                        temp += new string(source_char);
                        source_char = temp.ToCharArray();
                    }
                }
                else
                {
                    source_char[source_char.Count() - 1] = (char)((int)(source_char[source_char.Count() - 1]) + 1);
                }


                result = new string(source_char);
                return result;
            }

            public void Dispose()
            {
                exapp.Quit(); 
            }
       
    }
}
