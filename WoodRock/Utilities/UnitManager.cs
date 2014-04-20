using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoodRock.Utilities
{
    public class UnitManager
    {
        public static List<Models.Human> HumanUnits = new List<Models.Human>();
        public static List<Models.Animal> AnimalUnits = new List<Models.Animal>();
        public static List<Models.Enemy> EnemyUnits = new List<Models.Enemy>();

        public static void LoadUnits(string[] fileLines)
        {
            List<string> humanLines = new List<string>();
            List<string> animalLines = new List<string>();
            List<string> enemyLines = new List<string>();

            int lineNum = 0;

            if (fileLines[0] != "[sfv = 1.5]")
            {
                throw new NotSupportedException("Only save version 1.5 is supported");
            }

            Models.Preferences.startNewLoad(fileLines[1]);

            // get human lines
            int humanCnt = Convert.ToInt32(fileLines[2]);
            lineNum = 3;
            for (int i = 0; i < humanCnt; i++)
            {
                string uLine = fileLines[i + lineNum];
                humanLines.Add(uLine);
            }
           
            // get animal lines
            int animalCnt = Convert.ToInt32(fileLines[2 + humanCnt + 1]);
            lineNum = lineNum + humanCnt + 1;
            for (int i = 0; i < animalCnt; i++)
            {
                string uLine = fileLines[i + lineNum];
                animalLines.Add(uLine);
            }

            // get enemy lines
            int enemyCnt = Convert.ToInt32(fileLines[2 + humanCnt + 1 + animalCnt + 1]);
            lineNum = lineNum + animalCnt + 1;
            for (int i = 0; i < enemyCnt; i++)
            {
                string uLine = fileLines[i + lineNum];
                enemyLines.Add(uLine);
            }


            // load humans
            foreach (var line in humanLines)
            {
                HumanUnits.Add(Models.Human.LoadFromSaveLine(line));
            }

            // load animals
            foreach (var line in animalLines)
            {
                AnimalUnits.Add(Models.Animal.LoadFromSaveLine(line));
            }

            // load enemies
            foreach (var line in enemyLines)
            {
                EnemyUnits.Add(Models.Enemy.LoadFromSaveLine(line));
            }
        }

    }
}
