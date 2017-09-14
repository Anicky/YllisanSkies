using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace RaverSoft.YllisanSkies
{
    public class SaveSystem
    {
        private const string SAVE_DIRECTORY_NAME = "";
        private const string SAVE_NAME = "Save";
        public const int MAX_NUMBER_OF_SAVES = 4;

        private string getSaveDirectoryPath()
        {
            return Application.persistentDataPath + "/" + SAVE_DIRECTORY_NAME;
        }

        private string getSavePath(int saveNumber)
        {
            return getSaveDirectoryPath() + "/" + SAVE_NAME + saveNumber.ToString() + ".dat";
        }

        private string getScreeshotSavePath(int saveNumber)
        {
            return getSaveDirectoryPath() + "/" + SAVE_NAME + saveNumber.ToString() + ".jpg";
        }

        public bool hasGameSaves()
        {
            bool hasGameSaves = false;
            if (Directory.Exists(getSaveDirectoryPath()))
            {
                for (int i = 1; i <= MAX_NUMBER_OF_SAVES; i++)
                {
                    if (File.Exists(getSavePath(i)))
                    {
                        try
                        {
                            SaveData saveData = load(i);
                            hasGameSaves = true;
                        }
                        catch (Exception e)
                        {
                        }
                        break;
                    }
                }
            }
            return hasGameSaves;
        }

        public void save(int saveNumber, SaveData saveData, Texture2D screenshot)
        {
            saveData.setScreenshotPath(getScreeshotSavePath(saveNumber));
            File.WriteAllBytes(getScreeshotSavePath(saveNumber), screenshot.EncodeToJPG());
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(getSavePath(saveNumber));
            bf.Serialize(file, saveData);
            file.Close();
        }

        public SaveData load(int saveNumber)
        {
            SaveData saveData = null;
            if (File.Exists(getSavePath(saveNumber)))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(getSavePath(saveNumber), FileMode.Open);
                try
                {
                    saveData = (SaveData)bf.Deserialize(file);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    file.Close();
                }
            }
            return saveData;
        }

    }
}