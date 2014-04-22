using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using WoodRock.Models;
using WoodRock.Utilities;

namespace WoodRock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Models.SavedGame SavedGame { get; set; }


        public List<Human> Units { get; set; }
        
        public List<string> FileLines { get; set; }
        public List<string> UnitLines { get; set; }
        
        public MainWindow(Models.SavedGame savedGame)
        {
            SavedGame = savedGame;
            
            InitializeComponent();

            Units = new List<Human>();
            FileLines = new List<string>();
            UnitLines = new List<string>();
            UnitViewModel model = new UnitViewModel();
            model.SavedGame = savedGame;
            ReadFile();
            this.DataContext = model;


            var fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            this.Title += " " + fvi.FileVersion;
            //this.DataContext = Units.OrderBy(x => x.UnitName);

            //var profCounts = Units.GroupBy(x => x.ProfessionName).Select(x => new { Name = x.Key, Count = x.Count() });
            //uxProfCounts.DataContext = profCounts;

            //uxSettelmentName.Content = model.SavedGame.SaveName;
        }

        public void ReadFile()
        {
            // Load Resources
            var reFileLines = new List<string>();
            using (StreamReader sr = new StreamReader(System.IO.Path.Combine(SavedGame.Path, "re.sav")))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    reFileLines.Add(line);
                }
            }
            ResourceManager.LoadResources(reFileLines.ToArray());
            
            
            // Load Units
            string path = System.IO.Path.Combine(SavedGame.Path, "un.sav");
            using (StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    FileLines.Add(line);
                }
            }

            UnitManager.LoadUnits(FileLines.ToArray());



            //uxOutput.Text += ResourceManager.ResourceList.Count.ToString() + Environment.NewLine;
            //uxOutput.Text += ResourceManager.GetSaveString() + Environment.NewLine;


            
            int unitCnt = Convert.ToInt32(FileLines[2]);
            for (int i = 0; i < unitCnt; i++)
            {
                string uLine = FileLines[i + 3];
                UnitLines.Add(uLine);
            }


            int uCnt = 1;
            foreach (var unitLine in UnitLines)
            {
                Human human = Human.LoadFromSaveLine(unitLine);
                
                var unit = unitLine.Split('/').ToList();        
                uxOutput.Text += "UNIT " + uCnt.ToString() + Environment.NewLine;
                                
                //4 preferences
                uxOutput.Text += "preferences: " + unit[4] + Environment.NewLine;

                foreach (var p in human.Preferences._preferences)
                {
                    uxOutput.Text += p + Environment.NewLine;
                }

                uxOutput.Text += "sleep.schedule:" + human.Preferences["sleep.schedule"] + Environment.NewLine;

                uxOutput.Text += "Name: " + unit[10] + Environment.NewLine;

                //15 inventory
                foreach (var item in human.Inventory)
                {
                    uxOutput.Text += "INV: " + item.Key.ToString() + "-" + item.Value.ToString() + Environment.NewLine;
                }           

                Units.Add(human);

                uxOutput.Text += Environment.NewLine + Environment.NewLine;
                uCnt++;
            }

        }


        void OpenCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            String command, targetobj;
            command = ((RoutedCommand)e.Command).Name;
            targetobj = ((FrameworkElement)target).Name;
            //MessageBox.Show("The " + command + " command has been invoked on target object " + targetobj);
            StartupWindow startup = new StartupWindow(this);
            startup.Show();
            //this.Close();
        }


        void SaveCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            //String command, targetobj;
            //command = ((RoutedCommand)e.Command).Name;
            //targetobj = ((FrameworkElement)target).Name;
            //MessageBox.Show("The " + command + " command has been invoked on target object " + targetobj);


            // Save re.sav file
            using (StreamWriter sw = new StreamWriter(System.IO.Path.Combine(SavedGame.Path, "re.sav")))
            {
                sw.Write(WoodRock.Utilities.ResourceManager.GetSaveFile());
            }


            MessageBox.Show(SavedGame.SaveName + " Saved.");
        }


        void CloseCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        void SaveCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        void CmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);
            DataGridCell RowColumn = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
            string cellValue = ((TextBlock)RowColumn.Content).Text;

            Human selected = Units.First(x => x.UnitName == cellValue);
            uxInfoPanel.DataContext = selected;
        }
    }
}
