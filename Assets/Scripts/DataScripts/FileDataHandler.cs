using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    private bool useEncryption = false;
    private readonly string encryptionKeyWord = "asdgdfagfg";

    private readonly string backupExtension = ".bak";

    public FileDataHandler(string dirPath, string fileName, bool encryption)
    {
        this.dataDirPath = dirPath;
        this.dataFileName = fileName;
        this.useEncryption = encryption;
    }

    public GameData Load(bool allowRestoreFromBackup = true)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad ="";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                try
                {
                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad); 
                }
                catch
                {
                    Debug.LogWarning("couldn't load data");
                    dataToLoad = EncryptDecrypt(dataToLoad);

                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad); 
                }
            }
            catch (Exception e)
            {
                if (allowRestoreFromBackup)
                {
                    Debug.LogWarning("Failed to load data. Attempting to load backup files.");
                    bool LoadBackupSuccess = LoadBackupData(fullPath);
                    if (LoadBackupSuccess)
                    {
                       loadedData = Load(false);
                    }
                }
                else
                {
                    Debug.LogError("Backup did not work: " + e);
                }
            }
        }

        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        string backupPath = fullPath + backupExtension;
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

            GameData verifiedData = Load();
            if (verifiedData != null)
            {
                File.Copy(fullPath, backupPath, true);
            }
            else
            {
                Debug.LogError("Save file could not be verified! Backup file could not be created.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file : " + fullPath + "\n" + e);
        }
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionKeyWord[i % encryptionKeyWord.Length]);
        }
        return modifiedData;
    }

    private bool LoadBackupData(string full_Path)
    {
        bool successfullyLoaded = false;
        string backup_path = full_Path + backupExtension;
        try
        {
            if (File.Exists(backup_path))
            {
                File.Copy(backup_path, full_Path, true);
                successfullyLoaded = true;
                Debug.LogWarning("Had to load back to backup files.");
            }
            else
            {
                Debug.LogError("Tried to load back to backup files, but no backup found.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured trying to load back to backup file: " + e);
        }
        return successfullyLoaded;
    }

}
