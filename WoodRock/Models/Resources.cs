using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WoodRock
{
    public static class Resources
    {
        public static List<Resource> ResourceList { get; set; }


        static Resources()
        {
            ResourceList = new List<Resource>();
            ReadResourceFile();
        }


        public static void ReadResourceFile()
        {
            string path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "resource_list.csv");
            using (StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] item = line.Split(',');
                    ResourceList.Add(new Resource { Name = item[0], ResourceType = item[1] });
                }
            }
        }
    }


    public class Resource
    {
        public string Name { get; set; }
        public string ResourceType { get; set; }

        public Resource()
        {

        }
    }
}
