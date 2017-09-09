using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace RaverSoft.YllisanSkies
{
    public class SaveSystem
    {
        public void save(string saveName, SaveData saveData)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + saveName + ".dat");
            bf.Serialize(file, saveData);
            file.Close();
        }

        public SaveData load(string saveName)
        {
            SaveData saveData = null;
            if (File.Exists(Application.persistentDataPath + "/" + saveName + ".dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + saveName + ".dat", FileMode.Open);
                saveData = (SaveData)bf.Deserialize(file);
                file.Close();
            }
            return saveData;
        }

    }
}