using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace RaverSoft.YllisanSkies.Utils
{
    public class CSVReader
    {
        public static List<Dictionary<string, string>> getItemsFromFile(string fileName)
        {
            TextAsset textAsset = Resources.Load(fileName) as TextAsset;
            string[] lines = Regex.Split(textAsset.text, "\n|\r|\r\n");
            string[] headers = splitCSVline(lines[0]);
            List<Dictionary<string, string>> items = new List<Dictionary<string, string>>();
            for (int i = 1; i < lines.Length; i++)
            {
                if (lines[i].Trim() != "")
                {
                    string[] line = splitCSVline(lines[i]);
                    Dictionary<string, string> item = new Dictionary<string, string>();
                    for (int j = 0; j < headers.Length; j++)
                    {
                        item.Add(headers[j], line[j]);
                    }
                    items.Add(item);
                }
            }
            return items;
        }

        private static string[] splitCSVline(string line)
        {
            return line.Split(';');
        }
    }
}