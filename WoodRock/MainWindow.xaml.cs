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
            //this.DataContext = Units.OrderBy(x => x.UnitName);

            var profCounts = Units.GroupBy(x => x.ProfessionName).Select(x => new { Name = x.Key, Count = x.Count() });
            uxProfCounts.DataContext = profCounts;

            uxSettelmentName.Content = model.SavedGame.SaveName;
        }

        public void ReadFile()
        {
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



            if (FileLines[0] != "[sfv = 1.5]")
            {
                throw new NotSupportedException("Only version 1.5 is supported");
            }

            Models.Preferences.startNewLoad(FileLines[1]);

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
                string[] inv1 = unit[15].Split('-');
                if (inv1[0] != string.Empty)
                {
                    string[] inv2 = inv1[0].Split('|');
                    for (int idx2 = 0; idx2 < inv2.Length; ++idx2)
                    {
                        if (!(inv2[idx2] == string.Empty))
                        {
                            string[] inv3 = inv2[idx2].Split(',');
                            if (!(inv3[0] == string.Empty) && !(inv3[1] == string.Empty))
                            {
                                int itemID = Convert.ToInt32(inv3[0][0]) - 128;
                                int itemCnt = Convert.ToInt32(inv3[1][0]) - 128;

                                uxOutput.Text += "INV: " + itemID.ToString() + "-" + itemCnt.ToString() + Environment.NewLine;
                            }
                        }
                    }
                }
                if (inv1[1] != string.Empty)
                {
                    string[] inv2 = inv1[1].Split('|');
                    for (int idx2 = 0; idx2 < inv2.Length; ++idx2)
                    {
                        if (!(inv2[idx2] == string.Empty))
                        {
                            string[] inv3 = inv2[idx2].Split(',');
                            if (!(inv3[0] == string.Empty) && !(inv3[1] == string.Empty))
                            {
                                int itemID = Convert.ToInt32(inv3[0][0]) - 128;
                                int itemCnt = Convert.ToInt32(inv3[1][0]) - 128;

                                uxOutput.Text += "INV2: " + itemID.ToString() + "-" + itemCnt.ToString() + Environment.NewLine;
                            }
                        }
                    }
                }
                

                //16 x
                uxOutput.Text += "X: " + (float.Parse(unit[16]) - 128f).ToString() + Environment.NewLine;

                //17 y
                uxOutput.Text += "Y: " + (float.Parse(unit[17]) - 128f).ToString() + Environment.NewLine;

                //18 z
                uxOutput.Text += "Z: " + (float.Parse(unit[18]) - 128f).ToString() + Environment.NewLine;

                //19 euler
                uxOutput.Text += "euler: " + (float.Parse(unit[19]) - 128f).ToString() + Environment.NewLine;

                //20 time to eat
                uxOutput.Text += "time to eat: " + (float.Parse(unit[20]) - 128f).ToString() + Environment.NewLine;


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
            String command, targetobj;
            command = ((RoutedCommand)e.Command).Name;
            targetobj = ((FrameworkElement)target).Name;
            MessageBox.Show("The " + command + " command has been invoked on target object " + targetobj);
        }


        void CloseCmdExecuted(object target, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        void SaveCmdCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
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
