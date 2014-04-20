using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoodRock.Models
{
    public class Preferences
    {
        public HashSet<string> _preferences = new HashSet<string>();
        protected static HashSet<string> _preferencesSave;
        protected static List<string> _preferencesSaveOrdered;

        public bool this[string pref]
        {
            get
            {
                pref = pref.ToLower();
                return this._preferences.Contains(pref);
            }
            set
            {
                pref = pref.ToLower();
                if (value)
                    this._preferences.Add(pref);
                else
                    this._preferences.Remove(pref);
            }
        }

        public string getSaveLine()
        {
            ulong num = 0uL;
            foreach (string pref in this._preferences)
            {
                int? num2 = null;
                int? num3;
                if (Preferences._preferencesSave.Add(pref))
                {
                    Preferences._preferencesSaveOrdered.Add(pref);
                    num3 = new int?(Preferences._preferencesSaveOrdered.Count - 1);
                }
                else
                {
                    num3 = new int?(Preferences._preferencesSaveOrdered.FindIndex((string x) => x.Equals(pref)));
                }
                if (num3.HasValue)
                {
                    ulong num4 = 1uL << num3.Value;
                    num |= num4;
                }
            }
            return Convert.ToString(num);
        }

        public void loadSaveLine(string line)
        {
            this._preferences = new HashSet<string>();
            if (Preferences._preferencesSaveOrdered.Count == 0)
                return;
            ulong num1 = Convert.ToUInt64(line);
            for (int index1 = 0; index1 < Preferences._preferencesSaveOrdered.Count; ++index1)
            {
                string index2 = Preferences._preferencesSaveOrdered[index1];
                ulong num2 = 1UL << index1;
                if (((long)num1 & (long)num2) != 0L)
                    this[index2] = true;
            }
        }

        public static void startNewSave()
        {
            Preferences._preferencesSave = new HashSet<string>();
            Preferences._preferencesSaveOrdered = new List<string>();
        }

        public static string getPrefStringList()
        {
            string str = string.Empty;
            for (int index = 0; index < Preferences._preferencesSaveOrdered.Count; ++index)
            {
                str = str + Preferences._preferencesSaveOrdered[index];
                if (index < Preferences._preferencesSaveOrdered.Count - 1)
                    str = str + ",";
            }
            return str;
        }

        public static void startNewLoad(string prefList)
        {
            Preferences._preferencesSave = new HashSet<string>();
            Preferences._preferencesSaveOrdered = new List<string>();
            string[] strArray1;
            if (prefList.Contains(","))
            {
                string str = prefList;
                char[] chArray = new char[1];
                int index = 0;
                int num = 44;
                chArray[index] = (char)num;
                strArray1 = str.Split(chArray);
            }
            else
            {
                string[] strArray2 = new string[1];
                int index = 0;
                string str = prefList;
                strArray2[index] = str;
                strArray1 = strArray2;
            }
            for (int index = 0; index < strArray1.Length; ++index)
            {
                if (!(strArray1[index] == string.Empty) && Preferences._preferencesSave.Add(strArray1[index]))
                    Preferences._preferencesSaveOrdered.Add(strArray1[index]);
            }
        }
    }
}
