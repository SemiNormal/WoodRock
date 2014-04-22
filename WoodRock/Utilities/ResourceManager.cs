using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace WoodRock.Utilities
{
    public class ResourceManager
    {
        //public static List<VillageResource> ResourceList = new List<VillageResource>(); 
        public static ObservableCollection<VillageResource> ResourceList = new ObservableCollection<VillageResource>();
        public static string Wealth { get; set; }
        public static string FoodDifferenceCount { get; set; }
        public static string FoodDifference { get; set; }

        public static void LoadResources(string[] fileLines)
        {
            ResourceList.Clear();

            if (!String.IsNullOrEmpty(fileLines[0]))
            {
                for (int i = 0; i < fileLines[0].Length; i++)
                {
                    int cnt = Convert.ToInt32(fileLines[0][i]) - 128;
                    var resource = Resources.ResourceList[i];
                    ResourceList.Add(new VillageResource
                    {
                        ItemId = i + 1,
                        Name = resource.Name,
                        Quantity = cnt
                    });
                }
            }

            Wealth = fileLines[1];
            FoodDifferenceCount = fileLines[2];
            FoodDifference = fileLines[3];
        }


        public static string GetSaveFile()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(GetSaveString());
            sb.AppendLine(Wealth);
            sb.AppendLine(FoodDifferenceCount);
            sb.AppendLine(FoodDifference);
            return sb.ToString();
        }

        private static string GetSaveString()
        {
            string matLine = string.Empty;
            for (int i = 0; i < ResourceList.Count; i++)
            {
                matLine = matLine + (object)Convert.ToChar(ResourceList[i].Quantity + 128);
            }

            return matLine;
        }
    }


    public class VillageResource : INotifyPropertyChanged
    {
        private int itemId;
        private string name;
        private int quantity;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public int ItemId { 
            get { return itemId; }
            set
            {
                if (value != this.itemId)
                {
                    this.itemId = value;
                    NotifyPropertyChanged("ItemId");
                }
            }
        }

        public string Name {
            get { return name; }
            set
            {
                if (value != this.name)
                {
                    this.name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public int Quantity {
            get { return quantity; }
            set
            {
                if (value != this.quantity)
                {
                    this.quantity = value;
                    NotifyPropertyChanged("Quantity");
                }
            } 
        }
    }
}
