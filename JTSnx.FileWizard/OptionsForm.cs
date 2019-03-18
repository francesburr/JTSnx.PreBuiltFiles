using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace JTSnx.FileWizard
{
    public partial class OptionsForm : Form
    {
        private IEnumerable<string> _projectList;

        public OptionsForm(IEnumerable<string> projectList)
        {
            InitializeComponent();

            _projectList = projectList;
        }

        public string SelectedDALProject => DALProject.SelectedValue.ToString();

        public string SelectedBLLProject => BLLProject.SelectedValue.ToString();

        public string SelectedAPIProject => APIProject.SelectedValue.ToString();


        protected override void OnLoad(EventArgs e)
        {
            DALProject.DataSource = _projectList.ToList();
            DALProject.SelectedItem = _projectList.FirstOrDefault(it => it.Contains("DAL"));

            BLLProject.DataSource = _projectList.ToList();
            BLLProject.SelectedItem = _projectList.FirstOrDefault(it => it.Contains("BLL"));

            APIProject.DataSource = _projectList.ToList();
            APIProject.SelectedItem = _projectList.FirstOrDefault(it => it.Contains("API"));

            base.OnLoad(e);
        }
    }
}
