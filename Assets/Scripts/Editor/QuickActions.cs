using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuickActions : EditorWindow
{
    [MenuItem("Window/Quick Actions")]
    public static void CreateWindow()
    {
        CreateWindow<QuickActions>();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Build All"))
        {
            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, "Builds/Windows/Ammoracked.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
            BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, "Builds/WebGL", BuildTarget.WebGL, BuildOptions.None);
        }
    }

    
}
