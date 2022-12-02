using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class Save : IDisposable
{
    public int maxScore;
    public int lastMaxScore;
    public int maxTanksDead;
    public float maxTimeAlive;

    public static Save Instance { get; private set; }

    const string SavePath = "/Saves/";
    const string SaveFileName = "main.sav";

    static Save ()
    {
        Application.quitting += WriteSave;
    }

    Save ()
    {
        maxTanksDead = 0;
    }

    public static Save Get ()
    {
        if (Instance != null) return Instance;
        else return ReadSave();
    }

    public static Save ReadSave ()
    {
        if (File.Exists(Application.dataPath + SavePath + SaveFileName))
        {
            var bf = new BinaryFormatter();
            using (var fs = new FileStream(Application.dataPath + SavePath + SaveFileName, FileMode.Open))
            {
                Instance = bf.Deserialize(fs) as Save;
            }
        }
        else
        {
            Instance = new Save();
        }

        return Instance;
    }

    public static void WriteSave ()
    {
        if (!Directory.Exists(Application.dataPath + SavePath))
        {
            Directory.CreateDirectory(Application.dataPath + SavePath);
        }

        var bf = new BinaryFormatter();
        using (var fs = new FileStream(Application.dataPath + SavePath + SaveFileName, FileMode.OpenOrCreate))
        {
            bf.Serialize(fs, Instance);
        }
    }

    public void Dispose()
    {
        WriteSave();
    }
}