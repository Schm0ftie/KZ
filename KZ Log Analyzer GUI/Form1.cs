using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KZLogAnalyzer.Data;

namespace KZLogAnalyzer.GUI
{
    public partial class Form1 : Form
    {
        private string VersionNr = "0.1";
        private int BuildNr = 1;
        private DataContainer DHolder;

        public Form1()
        {
            InitializeComponent();
            this.Text = "KZ Log Analyzer GUI - v." + VersionNr;
            DHolder = new DataContainer();
            DHolder.LoadData();
            //GridViewCoreJumps.DataSource = DHolder.GetJumpList(DHolder.CoreList);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if (TextBoxPath.Text == String.Empty)
                return;
            DHolder.LoadLog(TextBoxPath.Text);
            DataGridJumps.DataSource = DHolder.ParsedList;
            
            
        }

        private void ButtonSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFileDialog = new OpenFileDialog();
            oFileDialog.Filter = "Log File|*.log";
            oFileDialog.Multiselect = false;
            DialogResult oResult = oFileDialog.ShowDialog();
            if (oResult == DialogResult.OK)
            {
                TextBoxPath.Text = oFileDialog.FileName;
            }  
        }

        private void DataGridJumps_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridDetails.DataSource = DHolder.ParsedList.Count > e.RowIndex && e.RowIndex >= 0 ? DHolder.ParsedList[e.RowIndex].Maps : null;
        }

        private void ButtonExportXML_Click(object sender, EventArgs e)
        {
            DHolder.Save("..\\data.xml");
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string test = DHolder.GetPlayerName(DHolder.ParsedList);
        }
    }
}
