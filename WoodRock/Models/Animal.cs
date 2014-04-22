using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoodRock.Models
{
    public class Animal
    {
        public string UnitName { get; set; }
        public float Hunger { get; set; }
        public float ResourceCounter1 { get; set; }
        public float ResourceCounter2 { get; set; }
        public float ResourceCounter3 { get; set; }
        public bool Domesticated { get; set; }
        public bool TagDomesticated { get; set; }
        public bool IsDead { get; set; }
        public bool Slaughter { get; set; }
        public float XPos { get; set; }
        public float YPos { get; set; }
        public float ZPos { get; set; }
        public float Euler { get; set; }


        public static Animal LoadFromSaveLine(string line)
        {
            var unit = line.Split('/').ToList();
            var animal = new Animal();

            animal.UnitName = unit[0];
            animal.XPos = float.Parse(unit[1]) - 128f;
            animal.YPos = float.Parse(unit[2]) - 128f;
            animal.ZPos = float.Parse(unit[3]) - 128f;
            animal.Euler = float.Parse(unit[4]) - 128f;
            animal.Hunger = float.Parse(unit[5]);
            animal.ResourceCounter1 = float.Parse(unit[6]);
            animal.ResourceCounter2 = float.Parse(unit[7]);
            animal.ResourceCounter3 = float.Parse(unit[8]);
            animal.Domesticated = bool.Parse(unit[9]);
            animal.Slaughter = bool.Parse(unit[10]);
            animal.IsDead = bool.Parse(unit[11]);
            try
            {
                animal.TagDomesticated = bool.Parse(unit[12]);
            }
            catch
            {
            }

            return animal;
        }
    }
}
