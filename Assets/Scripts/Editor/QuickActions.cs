using System;
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
            if (EditorUtility.DisplayDialog("Build All", "Are you Sure?", "Yes", "No"))
            {
                BuildAll();
            }
        }

        if (GUILayout.Button("Push Builds to Itch"))
        {
            if (EditorUtility.DisplayDialog("Push Builds to Itch", "Are you Sure?", "Yes", "No"))
            {
                BuildAllAndPush();
            }
        }
    }

    private void BuildAllAndPush()
    {
        if (EditorUtility.DisplayDialog("Push Builds to Itch", "Rebuild All", "Yes", "Use Old Build"))
        {
            BuildAll();
        }

        var butlerCommand = $"/C c:/butler/butler.exe push \"Builds/Windows\" boschdog03/ammoracked:win --userversion {Application.version}";
        System.Diagnostics.Process.Start("CMD.exe", butlerCommand);

        butlerCommand = $"/C c:/butler/butler.exe push \"Builds/WebGL\" boschdog03/ammoracked:html5 --userversion {Application.version}";
        System.Diagnostics.Process.Start("CMD.exe", butlerCommand);
    }

    private static void BuildAll()
    {
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, "Builds/Windows/Ammoracked.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, "Builds/WebGL", BuildTarget.WebGL, BuildOptions.None);
    }
}
