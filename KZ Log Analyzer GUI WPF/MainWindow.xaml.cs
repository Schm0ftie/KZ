using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KZLogAnalyzer;
using KZLogAnalyzer.Data;
using Microsoft.Win32;

namespace KZ_Log_Analyzer_GUI_WPF
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataContainer DHolder;
        private DataProvider DProvider;
        private CollectionViewSource ImportViewSource;
        private CollectionViewSource JumpViewSource;
        private CollectionViewSource StrafeViewSource1;
        private CollectionViewSource StrafeViewSource2;
        private CheckBox[] JumpFilter;
        private Button[] JumpPBs;
        private string LastLoadedPath;

        public static List<float> TickrateList = new List<float>() { 64f, 102f, 128f };
        public static List<string> JumpList = Enum.GetNames(typeof(JumpType)).ToList();

        public MainWindow()
        {

            InitializeComponent();
            DHolder = new DataContainer();
            DHolder.LoadData();
            DProvider = new DataProvider(DHolder);
            InitFilterArray();
            InitButtonPBArray();


            ImportViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSourceImport"));
            JumpViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSourceJumps"));
            StrafeViewSource1 = (CollectionViewSource)(FindResource("ItemCollectionViewSourceStrafe1"));
            StrafeViewSource2 = (CollectionViewSource)(FindResource("ItemCollectionViewSourceStrafe2"));

            JumpViewSource.Source = DProvider.GetFilteredJumpList(128f, JumpType.LongJump);

            ComboBoxJumpsFilterTickrate.ItemsSource = TickrateList;
            ComboBoxJumpsFilterTickrate.SelectedIndex = 2;
            ComboBoxJumpsFilterJumptype.ItemsSource = JumpList;
            ComboBoxJumpsFilterJumptype.SelectedIndex = 0;

            UpdatePersonalBest(128f);
        }

        private void ImportFilter(object sender, FilterEventArgs e)
        {
            Jump j = e.Item as Jump;
            if(j != null)
            {
                bool bJump = false;
                bool bName = false;
                bJump = JumpFilter[(int)j.JumpType].IsChecked == true ;
 
                if(ComboBoxPlayNames.SelectedValue == null || ComboBoxPlayNames.SelectedValue.ToString() == "All")
                    bName = true;
                else
                {
                    if (j.PlayerName.Equals(ComboBoxPlayNames.SelectedValue.ToString()))
                       bName = true;
                }

                e.Accepted = bJump && bName;
            }
        }

        private void CheckBoxJumpFilter(object sender, RoutedEventArgs e)
        {
            if(ImportViewSource != null && ImportViewSource.View != null)
             ImportViewSource.View.Refresh();
        }

        private void ButtonImport_Click(object sender, RoutedEventArgs e)
        {
            if (ImportViewSource.View == null || ((List<Jump>)ImportViewSource.Source).Count == 0)
                MessageBox.Show("No data for import available.");

            ImportFilter oImportFilter = new ImportFilter();
            oImportFilter.Tickrate64 = CheckBoxTickrate64.IsChecked == true;
            oImportFilter.Tickrate102 = CheckBoxTickrate102.IsChecked == true;
            oImportFilter.Tickrate128 = CheckBoxTickrate128.IsChecked == true;
            oImportFilter.JumpLJ = JumpFilter[(int)JumpType.LongJump].IsChecked == true;
            oImportFilter.JumpBhop = JumpFilter[(int)JumpType.Bhop].IsChecked == true;
            oImportFilter.JumpMbhop = JumpFilter[(int)JumpType.MultiBhop].IsChecked == true;
            oImportFilter.JumpDHop = JumpFilter[(int)JumpType.DropBhop].IsChecked == true;
            oImportFilter.JumpWJ = JumpFilter[(int)JumpType.WeirdJump].IsChecked == true;
            oImportFilter.JumpLAJ = JumpFilter[(int)JumpType.LadderJump].IsChecked == true;
            oImportFilter.JumpCJ = JumpFilter[(int)JumpType.CountJump].IsChecked == true;
            oImportFilter.PlayerName = ComboBoxPlayNames.SelectedValue == null  ? "All" : ComboBoxPlayNames.SelectedValue.ToString();
            oImportFilter.UseMinDistance = CheckBoxIgnoreShort.IsChecked == true;
            List<Server> oFilteredList = DProvider.GetFilteredServerList(DHolder.ParsedList, oImportFilter);
            DHolder.ImportData(oFilteredList);
            DHolder.Save(DataContainer.DEFAULT_DATA_PATH);
            if (LastLoadedPath != String.Empty)
                DHolder.DeleteLog(LastLoadedPath);
            UpdatePersonalBest(128f);
            
        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog oFileDialog = new OpenFileDialog();
            oFileDialog.Filter = "Log File|*.log";
            oFileDialog.Multiselect = false;
            if(oFileDialog.ShowDialog() == true) {
                DHolder.LoadLog(oFileDialog.FileName);
                LastLoadedPath = oFileDialog.FileName;
                ImportViewSource.Source = DProvider.GetJumpList(true);
                ImportViewSource.Filter += ImportFilter;
                ImportViewSource.View.Refresh();
                List<string> names = DProvider.GetPlayerNames(true);
                names.Insert(0, "All");
                ComboBoxPlayNames.ItemsSource = names;
                ComboBoxPlayNames.SelectedIndex = 0;
            }
        }

        private void ComboBoxPlayNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ImportViewSource.View != null)
                ImportViewSource.View.Refresh();
        }

        private void InitFilterArray()
        {
            JumpFilter = new CheckBox[7];
            JumpFilter[(int) JumpType.LongJump] = CheckBoxJumpFilterLJ;
            JumpFilter[(int)JumpType.Bhop] = CheckBoxJumpFilterBhop;
            JumpFilter[(int)JumpType.MultiBhop] = CheckBoxJumpFilterMbhop;
            JumpFilter[(int)JumpType.DropBhop] = CheckBoxJumpFilterDJ;
            JumpFilter[(int)JumpType.WeirdJump] = CheckBoxJumpFilterWJ;
            JumpFilter[(int)JumpType.LadderJump] = CheckBoxJumpFilterLAJ;
            JumpFilter[(int)JumpType.CountJump] = CheckBoxJumpFilterCJ;
        }

        private void InitButtonPBArray()
        {
            JumpPBs = new Button[7];
            JumpPBs[0] = ButtonPbLJ;
            JumpPBs[1] = ButtonPbBhop;
            JumpPBs[2] = ButtonPbMbhop;
            JumpPBs[3] = ButtonPbDbhop;
            JumpPBs[4] = ButtonPbWJ;
            JumpPBs[5] = ButtonPbLAJ;
            JumpPBs[6] = ButtonPbCJ;
        }

        private void CheckBoxTickrate_Checked(object sender, RoutedEventArgs e)
        {
            if(ImportViewSource != null && ImportViewSource.View != null)
            {
                ImportViewSource.Source = DProvider.GetFilteredJumpList(true, CheckBoxTickrate64.IsChecked == true, CheckBoxTickrate102.IsChecked == true, CheckBoxTickrate128.IsChecked == true);
                ImportViewSource.View.Refresh();
            }
        }

        private void DataGridJumps_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGridJumps_MouseButtonUp(sender, e, true);
        }

        private void DataGridJumps_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGridJumps_MouseButtonUp(sender, e, false);
        }

        private void DataGridJumps_MouseButtonUp(object sender, MouseButtonEventArgs e, bool isLeftClick)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null) return;

            if (dep is DataGridCell)
            {
                DataGridCell cell = dep as DataGridCell;
                cell.Focus();

                while ((dep != null) && !(dep is DataGridRow))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }
                DataGridRow row = dep as DataGridRow;
                DataGridJumps.SelectedItem = row.DataContext;

                Jump oJump = DataGridJumps.SelectedItem as Jump;
                if (isLeftClick)
                {
                    StrafeViewSource1.Source = null;
                    StrafeViewSource1.Source = oJump.Strafes;
                    StrafeViewSource1.View.Refresh();
                }
                else
                {
                    StrafeViewSource2.Source = null;
                    StrafeViewSource2.Source = oJump.Strafes;
                    StrafeViewSource2.View.Refresh();
                }
            }
        }


        private void ComboBoxJumpsFilterTickrate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePersonalBest((float)ComboBoxJumpsFilterTickrate.SelectedValue);
            ComboBoxJumpsFilter_SelectionChanged(sender, e);
        }

        private void ComboBoxJumpsFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxJumpsFilterTickrate.SelectedIndex >= 0 && ComboBoxJumpsFilterJumptype.SelectedIndex >= 0)
            {
                JumpType oJumpType = (JumpType) Enum.Parse(typeof(JumpType), ComboBoxJumpsFilterJumptype.SelectedValue.ToString());
                float oTickrate = (float)ComboBoxJumpsFilterTickrate.SelectedValue;
                JumpViewSource.Source = DProvider.GetFilteredJumpList(oTickrate, oJumpType);
                JumpViewSource.View.Refresh();
            }
        }

        private void UpdatePersonalBest(float tickrate)
        {
            Jump[] pbs = new Jump[7];
            for(int i = 0; i < pbs.Length; i++)
            {
                pbs[i] = DProvider.GetMaxDistanceJump(tickrate, (JumpType)i);
                JumpPBs[i].Content = pbs[i] != null ? pbs[i].Distance.ToString() : "N/A";
            }

        }
    }
}
