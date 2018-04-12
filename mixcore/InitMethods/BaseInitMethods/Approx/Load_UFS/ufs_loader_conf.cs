using System.ComponentModel;
using FuzzySystem.FuzzyAbstract.conf;
using  Settings=BaseInitMethods.Properties.SettingsBase;
using System;
namespace FuzzySystem.SingletoneApproximate.AddGenerators.conf
{
    public class Load_from_UFS_conf : IGeneratorConf
    {
        


        public Load_from_UFS_conf(string file_name)
        {
            Settings.Default.Load_UFS_file_name = file_name;
            Settings.Default.Save();
          
        }
    
                [Description("Источник базы правил"), Category("Файл")]

        public string Файл_UFS
        {
            get { return Settings.Default.Load_UFS_file_name; }
            set
            {
                Settings.Default.Load_UFS_file_name=value;
                Settings.Default.Save();
            }


        }

                public void Init(int countVars)
                {
                    
                }

                public void loadParams(string param)
                {
                    throw (new NotImplementedException());
                }

    }
}