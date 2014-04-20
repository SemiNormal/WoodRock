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
using System.Windows.Shapes;
using System.IO;

namespace WoodRock
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        List<Models.SavedGame> SavedGames { get; set; }
        private MainWindow MainInstance = null;

        public StartupWindow() : this(null)
        {
           
        }

        public StartupWindow(MainWindow mainInstance)
        {
            MainInstance = mainInstance;
            InitializeComponent();
            SavedGames = new List<Models.SavedGame>();
            if(!String.IsNullOrEmpty(Properties.Settings.Default.SavePath))
            {
                uxSaveFile.Text = Properties.Settings.Default.SavePath;
                LoadSaves(Properties.Settings.Default.SavePath);
            }
        }


        private void LoadSaves(string path)
        {
            if (path.ToLower().EndsWith("saves.sav"))
            {
                SavedGames = new List<Models.SavedGame>();
                
                List<string> lines = new List<string>();
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        lines.Add(line);
                    }
                }

                int saveCnt = Convert.ToInt32(lines[0]);
                for (int i = 0; i < saveCnt; i++)
                {
                    string[] line = lines[i + 1].Split('/');
                    SavedGames.Add(new Models.SavedGame
                    {
                        SaveName = line[0],
                        Day = Convert.ToInt32(line[1]),
                        Villagers = Convert.ToInt32(line[2]),
                        LastSaved = line[3],
                        MapSize = line[4],
                        Path = System.IO.Path.Combine(path.Replace("saves.sav", ""), line[0])
                    });
                }

                uxSaveList.DataContext = SavedGames;
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".sav";
            dlg.Filter = "Saves File (*.sav)|*.sav";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                uxSaveFile.Text = filename;
                Properties.Settings.Default.SavePath = filename;
                Properties.Settings.Default.Save();
                LoadSaves(filename);
            }
        }


        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            string saveName = ((Button)sender).Tag.ToString();
            
            if (MainInstance != null)
            {
                MainInstance.Close();
            }

            var game = SavedGames.Single(x => x.SaveName == saveName);
            
            MainWindow main = new MainWindow(game);
            main.Show();
            this.Close();
        }
    }
}
