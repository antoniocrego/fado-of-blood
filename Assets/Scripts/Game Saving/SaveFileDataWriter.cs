using UnityEngine;
using System;
using System.IO;

public class SaveFileDataWriter
{
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";

    public bool CheckToSeeIfFileExists()
    {
        if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
        {
            //Debug.Log("Save file exists.");
            return true;
        }
        else
        {
            //Debug.Log("Save file does not exist.");
            return false;
        }
    }

    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));

    }

    public void CreateNewSaveFile(CharacterSaveData saveData)
    {
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("Creating save file at: " + savePath);

            string dataToStore = JsonUtility.ToJson(saveData, true);

            using FileStream fileStream = new FileStream(savePath, FileMode.Create);
            using StreamWriter writer = new StreamWriter(fileStream);
            writer.Write(dataToStore);
            Debug.Log("Save file created successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error creating save file: " + e.Message);
        }
    }

    public CharacterSaveData LoadSaveFile()
    {
        CharacterSaveData characterData = null;
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

        if (File.Exists(savePath))
        {
            try
            {
                string dataToLoad;

                using FileStream fileStream = new FileStream(savePath, FileMode.Open);
                using StreamReader reader = new StreamReader(fileStream);
                dataToLoad = reader.ReadToEnd();
                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
                //Debug.Log("Save file loaded successfully.");
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading save file: " + e.Message);
            }
        }
        else
        {
            //Debug.LogWarning("Save file does not exist.");
        }

        return characterData;
    }

}
