using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Mix_core.Forms
{
    public partial class FileMultiSelectForm : Form
    {
        public List<string> TraFiles { get; protected set; }
        public List<string> TstFiles { get; protected set; }


        public FileMultiSelectForm()
        {
            InitializeComponent();
            openFileDialog1.ValidateNames = false;
            openFileDialog1.CheckPathExists = false;
            if (Directory.Exists(Properties.Settings.Default.MultiSelectFolder))
            {
                openFileDialog1.FileName = Path.Combine(Properties.Settings.Default.MultiSelectFolder, " ");
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            // Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (TraView.Items.Count != TstView.Items.Count)
            {
                MessageBox.Show($"Количество обучающих выборок не равно количеству тестовых {Environment.NewLine}Исправьте это", "Неверное количество файлов", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            TraFiles = new List<string>(TraView.Items.Count);
            TstFiles = new List<string>(TstView.Items.Count);

            foreach (ListViewItem s in TraView.Items)
            {
                TraFiles.Add(Path.Combine (Properties.Settings.Default.MultiSelectFolder,s.Text));
            }

            foreach (ListViewItem s in TstView.Items)
            {
                TstFiles.Add(Path.Combine(Properties.Settings.Default.MultiSelectFolder, s.Text));
            }


            DialogResult = DialogResult.OK;
        }

        private void FolderSelectButton_Click(object sender, EventArgs e)
        {




            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (Directory.Exists(Path.GetDirectoryName(openFileDialog1.FileName)))
                {
                    Properties.Settings.Default.MultiSelectFolder = Path.GetDirectoryName(openFileDialog1.FileName);
                    Properties.Settings.Default.Save();
                    FolderTextBox.Text = Properties.Settings.Default.MultiSelectFolder;
                    FolderTextBox.RightToLeft = RightToLeft.Yes;
                    fillViews(Properties.Settings.Default.MultiSelectFolder);
                }

            }
        }


        private void fillViews(string path)
        {
            List<string> left = Directory.EnumerateFiles(path, "*.dat", SearchOption.TopDirectoryOnly).Select(x => Path.GetFileName(x)).ToList();
            left.AddRange(Directory.EnumerateFiles(path, "*.ufs", SearchOption.TopDirectoryOnly).Select(x => Path.GetFileName(x)).ToList());
            List<string> rigth = left.Where(x => x.Contains("tst.dat")).ToList();
            left.RemoveAll(x => rigth.Contains(x));
            rigth.AddRange(left.Where(x => x.Contains(".ufs")));
            left = left.Distinct().ToList();
            left.Sort();

            rigth = rigth.Distinct().ToList();
            rigth.Sort();


            TraView.Items.AddRange(left.Select(x => new ListViewItem(x)).ToArray());
            TstView.Items.AddRange(rigth.Select(x => new ListViewItem(x)).ToArray());
        }

        private void TraToTstButton_Click(object sender, EventArgs e)
        {
            Rewind(TraView, TstView);
        }

        private void TstToTraButton_Click(object sender, EventArgs e)
        {
            Rewind(TstView, TraView);
        }

        void Rewind(ListView Source, ListView Destination)
        {
            foreach (ListViewItem obj in Source.SelectedItems)
            {
                Source.Items.Remove(obj);
                Destination.Items.Add(obj);
            }

        }

        void UpList(ListView Source)
        {
            foreach (ListViewItem obj in Source.SelectedItems)
            {
                int i = Source.Items.IndexOf(obj);
                Source.Items.Remove(obj);
                if (i > 0) i--;
                Source.Items.Insert(i, obj);
            }
        }
        void DownList(ListView Source)
        {
            foreach (ListViewItem obj in Source.SelectedItems)
            {
                int i = Source.Items.IndexOf(obj);
                if ((i + 1) < Source.Items.Count) i++;

                Source.Items.Remove(obj);
                Source.Items.Insert(i, obj);
            }
        }

        private void TraUpButton_Click(object sender, EventArgs e)
        {
            UpList(TraView);
        }

        private void TstUpButton_Click(object sender, EventArgs e)
        {
            UpList(TstView);
        }

        private void TraDownButton_Click(object sender, EventArgs e)
        {
            DownList(TraView);
        }

        private void TstDownButton_Click(object sender, EventArgs e)
        {
            DownList(TstView);
        }


        void AddFile(ListView Source)
        {
            if (Open_samples_FD.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in Open_samples_FD.FileNames)
                {
                    if (File.Exists(file))
                    {
                        Source.Items.Add(Path.GetFileName(file));
                    }
                }
            }
        }

        private void TraAddButton_Click(object sender, EventArgs e)
        {
            AddFile(TraView);
        }

        private void TstAddButton_Click(object sender, EventArgs e)
        {
            AddFile(TstView);

        }
        void RemoveSelected(ListView Source)
        {
            foreach (ListViewItem obj in Source.SelectedItems)
            {
                Source.Items.Remove(obj);

            }
        }

        private void TraRemoveButton_Click(object sender, EventArgs e)
        {
            RemoveSelected(TraView);
        }

        private void TstRemoveButton_Click(object sender, EventArgs e)
        {
            RemoveSelected(TstView);
        }
    }
}
