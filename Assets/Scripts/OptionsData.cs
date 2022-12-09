using System;
using System.IO;
using UnityEngine;

public class OptionsData : IDisposable
{
    public Vector2Int resolution;
    public int fullscreenMode;

    static OptionsData instance;
    static string FilePath => Application.dataPath + "/options.json";

    public static OptionsData Get ()
    {
        if (instance != null) return instance;

        return instance = ReadOptionsFile();
    }

    private static OptionsData ReadOptionsFile()
    {
        if (!File.Exists(FilePath)) return new OptionsData();

        var data = File.ReadAllText(FilePath);
        return instance = JsonUtility.FromJson<OptionsData>(data);
    }

    public static void WriteOptionsFile ()
    {
        var data = JsonUtility.ToJson(Get());
        File.WriteAllText(FilePath, data);
    }

    public void Dispose()
    {
        WriteOptionsFile();
    }
}
