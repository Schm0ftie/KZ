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
        private LogReader Reader;
        private List<Jump> Jumps;
        private DataHolder DHolder;

        public Form1()
        {
            InitializeComponent();
            this.Text = "KZ Log Analyzer GUI - v." + VersionNr;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if (TextBoxPath.Text == String.Empty)
                return;
            Reader = new LogReader();
            Jumps = Reader.ReadLog(TextBoxPath.Text);
            DHolder = new DataHolder(Jumps);
            DataGridJumps.DataSource = Jumps;
            
            
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
            DataGridDetails.DataSource = Jumps.Count > e.RowIndex && e.RowIndex >= 0 ? Jumps[e.RowIndex].Strafes : null;
        }

        private void ButtonExportXML_Click(object sender, EventArgs e)
        {
            DHolder.Save("..\\test.xml");
        }
    }
}
