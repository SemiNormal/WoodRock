using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoodRock.Models
{
    public class Enemy
    {
        public string UnitName { get; set; }
        public int UnitType { get; set; }
        public float Hitpoints { get; set; }
        public float Hunger { get; set; }
        public float XPos { get; set; }
        public float YPos { get; set; }
        public float ZPos { get; set; }
        public float Euler { get; set; }


        public static Enemy LoadFromSaveLine(string line)
        {
            var unit = line.Split('/').ToList();
            var enemy = new Enemy();

            enemy.UnitName = unit[0];
            enemy.XPos = float.Parse(unit[1]) - 128f;
            enemy.YPos = float.Parse(unit[2]) - 128f;
            enemy.ZPos = float.Parse(unit[3]) - 128f;
            enemy.Euler = float.Parse(unit[4]) - 128f;
            enemy.Hitpoints = float.Parse(unit[5]);
            enemy.UnitType = Int32.Parse(unit[6]);

            return enemy;
        }
    }
}
