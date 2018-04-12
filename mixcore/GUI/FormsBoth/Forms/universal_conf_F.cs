using System;
using System.Windows.Forms;

namespace Mix_core.Forms
{
    public partial class universal_conf_form : Form
    {
        public universal_conf_form()
        {
            InitializeComponent();
        }

        private void OK_B_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void universal_conf_form_Load(object sender, EventArgs e)
        {

        }

        private void conf_algorithm_params_PG_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            conf_algorithm_params_PG.Refresh();
        }
    }
}
