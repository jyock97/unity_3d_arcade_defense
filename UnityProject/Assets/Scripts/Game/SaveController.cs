using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveController
{
    //TODO remove this
    //C:\Users\Jose\AppData\LocalLow\DefaultCompany\UnityProject
    private static readonly string SaveFile = $"{Application.persistentDataPath}/sdata";
    
    public static void SaveGameData(GameSaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream file = File.Create(SaveFile))
        {
            formatter.Serialize(file, data);
        }
    }

    public static GameSaveData LoadGameData()
    {
        if (File.Exists(SaveFile))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            GameSaveData data;
            using (FileStream file = File.Open(SaveFile, FileMode.Open))
            {
                data = formatter.Deserialize(file) as GameSaveData;
            }

            return data;
        }

        return null;
    }
}