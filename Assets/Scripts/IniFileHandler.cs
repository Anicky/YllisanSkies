using System.IO;
using System.Collections.Generic;
using System;

public class IniFileHandler
{
    private string path = "";
    private static Dictionary<string, Dictionary<string, string>> iniDictionary = new Dictionary<string, Dictionary<string, string>>();
    private static bool isInitialized = false;

    public IniFileHandler(string path)
    {
        this.path = path;
    }

    private bool firstRead()
    {
        if (File.Exists(path))
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                string section = "";
                string key = "";
                string value = "";
                while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                {
                    line.Trim();
                    if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        section = line.Substring(1, line.Length - 2);
                    }
                    else
                    {
                        string[] ln = line.Split(new char[] { '=' });
                        key = ln[0].Trim();
                        value = ln[1].Trim();
                    }
                    if (section == "" || key == "" || value == "")
                        continue;
                    populateIni(section, key, value);
                }
            }
        }
        return true;
    }

    private void populateIni(string section, string key, string value)
    {
        if (iniDictionary.ContainsKey(section))
        {
            if (iniDictionary[section].ContainsKey(key))
                iniDictionary[section][key] = value;
            else
                iniDictionary[section].Add(key, value);
        }
        else
        {
            Dictionary<string, string> newValue = new Dictionary<string, string>();
            newValue.Add(key.ToString(), value);
            iniDictionary.Add(section.ToString(), newValue);
        }
    }

    public void IniWriteValue(string section, string key, string value)
    {
        if (!isInitialized)
            firstRead();
        populateIni(section, key, value);
        //write ini
        WriteIni();
    }

    private void WriteIni()
    {
        using (StreamWriter sw = new StreamWriter(path))
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> sections in iniDictionary)
            {
                sw.WriteLine("[" + sections.Key.ToString() + "]");
                foreach (KeyValuePair<string, string> key in sections.Value)
                {
                    // value must be in one line
                    string value = key.Value.ToString();
                    value = value.Replace(Environment.NewLine, " ");
                    value = value.Replace("\r\n", " ");
                    sw.WriteLine(key.Key.ToString() + " = " + value);
                }
            }
        }
    }

    public string IniReadValue(string section, string key)
    {
        if (!isInitialized)
            firstRead();
        if (iniDictionary.ContainsKey(section))
        {
            if (iniDictionary[section].ContainsKey(key))
            {
                return iniDictionary[section][key];
            }
        }
        return null;
    }
}