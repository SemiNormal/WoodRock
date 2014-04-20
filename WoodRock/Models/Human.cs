using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoodRock.Models
{
    public class Human
    {
        public float InventoryMaxWeight { get; set; }
        public string UnitName { get; set; }
        public float Hitpoints { get; set; }
        public float MaxHP { get; set; }
        public float Fatigue { get; set; }
        public float Hunger { get; set; }
        public float Morale { get; set; }
        public int TimeToEat { get; set; }
        public Gender Gender { get; set; }
        public Equipment Equipment { get; set; }
        public List<HumanSkill> Skills { get; set; }
        public string ProfessionName { get; set; }
        public Models.Preferences Preferences { get; private set; }


        public Human()
        {
            Equipment = new Equipment();
            Skills = new List<HumanSkill>();
            Preferences = new Models.Preferences();
        }


        public static double ExperienceToLevel(int exp)
        {
            if (exp < 0)
                return 0.0;
            else
                return (Math.Sqrt((double)(4225 + 24 * exp)) - 65.0) / 30.0;
        }

        public static int ExperienceToLevelFloor(int exp)
        {
            return (int)Math.Floor(ExperienceToLevel(exp));
        }

        public static Profession GetProfessionFromId(int id)
        {
            return (Profession)id;
        }

        public static Human LoadFromSaveLine(string line)
        {
            var unit = line.Split('/').ToList();
            var human = new Human();

            //0 Max Inventory Weight
            human.InventoryMaxWeight = float.Parse(unit[0]);

            //1 skills
            for (int i = 0; i < 16; i++)
            {
                int skill = Convert.ToInt32(unit[1][i]) - 128;
                human.Skills.Add(new HumanSkill(Human.GetProfessionFromId(i).ToString(), skill));
            }

            //2 gender
            human.Gender = Gender.Male;
            if (Convert.ToBoolean(unit[2]))
            {
                human.Gender = Gender.Female;
            }

            //3 profession
            human.ProfessionName = unit[3];

            //4 preferences
            human.Preferences.loadSaveLine(unit[4]);
            //uxOutput.Text += "preferences: " + unit[4] + Environment.NewLine;

            //5 item
            human.Equipment.HandR.ItemId = (Convert.ToInt32(unit[5][0]) - 128);
            
            //6 item
            human.Equipment.HandL.ItemId = (Convert.ToInt32(unit[6][0]) - 128);
            
            //7 item
            human.Equipment.Helm.ItemId = (Convert.ToInt32(unit[7][0]) - 128);
            
            //8 item
            human.Equipment.Chest.ItemId = (Convert.ToInt32(unit[8][0]) - 128);
            
            //9 item
            human.Equipment.Boots.ItemId = (Convert.ToInt32(unit[9][0]) - 128);
            
            //10 name
            human.UnitName = unit[10];
            
            //11 hp
            human.Hitpoints = (float)((int)unit[11][0] - 128) / 10f;
            
            //12 fatigue
            human.Fatigue = (float)((int)unit[12][0] - 128) / 10000f;
            
            //13 hunger
            human.Hunger = (float)((int)unit[13][0] - 10128) / 10000f;
            
            //14 morale
            human.Morale = 0;

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
                            // requested item
                            int itemID = Convert.ToInt32(inv3[0][0]) - 128;
                            int itemCnt = Convert.ToInt32(inv3[1][0]) - 128;
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
                            // inventory item
                            int itemID = Convert.ToInt32(inv3[0][0]) - 128;
                            int itemCnt = Convert.ToInt32(inv3[1][0]) - 128;
                        }
                    }
                }
            }


            //16 x
            //uxOutput.Text += "X: " + (float.Parse(unit[16]) - 128f).ToString() + Environment.NewLine;

            //17 y
            //uxOutput.Text += "Y: " + (float.Parse(unit[17]) - 128f).ToString() + Environment.NewLine;

            //18 z
            //uxOutput.Text += "Z: " + (float.Parse(unit[18]) - 128f).ToString() + Environment.NewLine;

            //19 euler
            //uxOutput.Text += "euler: " + (float.Parse(unit[19]) - 128f).ToString() + Environment.NewLine;

            //20 time to eat
            //uxOutput.Text += "time to eat: " + (float.Parse(unit[20]) - 128f).ToString() + Environment.NewLine;

            return human;
        }
    }


    public class HumanSkill
    {
        public string Name { get; set; }
        public int Experience { get; set; }
        public double Level { get { return Human.ExperienceToLevel(Experience); } }
        public int LevelFloor { get { return Human.ExperienceToLevelFloor(Experience); } }
        public string Color
        {
            get
            {
                switch (Name.ToLower())
                {
                    case "adventurer":
                        return "BlueViolet";
                    case "archer":
                        return "Yellow";
                    case "builder":
                        return System.Windows.Media.Colors.Firebrick.ToString();
                    case "tailor":
                        return System.Windows.Media.Colors.Beige.ToString();
                    case "miner":
                        return System.Windows.Media.Colors.Blue.ToString();
                    case "herder":
                        return System.Windows.Media.Colors.Brown.ToString();
                    case "stonemason":
                        return System.Windows.Media.Colors.DarkGray.ToString();
                    case "woodchopper":
                        return System.Windows.Media.Colors.DarkGray.ToString();
                    case "blacksmith":
                        return System.Windows.Media.Colors.DimGray.ToString();
                    case "carpender":
                        return System.Windows.Media.Colors.Goldenrod.ToString();
                    case "infantry":
                        return System.Windows.Media.Colors.Crimson.ToString();
                    case "engineer":
                        return System.Windows.Media.Colors.Orange.ToString();
                    case "farmer":
                        return System.Windows.Media.Colors.YellowGreen.ToString();
                    case "fisherman":
                        return System.Windows.Media.Colors.Navy.ToString();
                    case "forager":
                        return System.Windows.Media.Colors.LawnGreen.ToString();
                    default:
                        return System.Windows.Media.Colors.AliceBlue.ToString();
                }
            }
        }

        public HumanSkill()
        {

        }

        public HumanSkill(string name, int exp)
        {
            Name = name;
            Experience = exp;
        }
    }


    public class Equipment
    {
        public EquipmentItem HandR { get; set; }
        public EquipmentItem HandL { get; set; }
        public EquipmentItem Helm { get; set; }
        public EquipmentItem Chest { get; set; }
        public EquipmentItem Boots { get; set; }

        public Equipment()
        {
            HandR = new EquipmentItem(0);
            HandL = new EquipmentItem(0);
            Helm = new EquipmentItem(0);
            Chest = new EquipmentItem(0);
            Boots = new EquipmentItem(0);
        }
    }


    public class EquipmentItem
    {
        public int ItemId { get; set; }
        public string ItemName
        {
            get
            {
                return (ItemId == 0) ? "" : Resources.ResourceList[ItemId].Name;
            }
        }

        public override string ToString()
        {
            return ItemId.ToString() + " - " + ItemName;
        }

        public EquipmentItem()
        {

        }

        public EquipmentItem(int id)
        {
            ItemId = id;
        }
    }
}
