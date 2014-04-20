using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoodRock.Models
{
    public class SavedGame
    {
        public string SaveName { get; set; }
        public int Villagers { get; set; }
        public int Day { get; set; }
        public string LastSaved { get; set; }
        public string MapSize { get; set; }
        public string Path { get; set; }
    }
}
