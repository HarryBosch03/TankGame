using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class QuickActions : EditorWindow
{
    [MenuItem("Window/Quick Actions")]
    public static void CreateWindow()
    {
        CreateWindow<QuickActions>();
    }

    Texture2D iconBG;
    Texture2D iconFG;

    private void OnGUI()
    {
        if (GUILayout.Button("Build All"))
        {
            if (EditorUtility.DisplayDialog("Build All", "Are you Sure?", "Yes", "No"))
            {
                BuildAll();
            }
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Push Builds to Itch"))
        {
            if (EditorUtility.DisplayDialog("Push Builds to Itch", "Are you Sure?", "Yes", "No"))
            {
                BuildAllAndPush();
            }
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Icon");

        EditorGUILayout.BeginHorizontal();
        iconBG = EditorGUILayout.ObjectField("Background", iconBG, typeof(Texture2D), false) as Texture2D;
        iconFG = EditorGUILayout.ObjectField("Foreground", iconFG, typeof(Texture2D), false) as Texture2D;
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Build Icons"))
        {
            BuildIcons();
        }
    }

    private void BuildIcons()
    {
        var compositeIcons = new Vector2Int[]
        {
            new Vector2Int(432, 432),
        };

        var staticIcons = new Vector2Int[]
        {
            new Vector2Int(192, 192),
        };

        string path = "/Sprites/Built Icons/";
        if (!Directory.Exists(Application.dataPath + path))
        {
            Directory.CreateDirectory(Application.dataPath + path);
        }

        Action<Texture2D, Vector2Int, string> resizeIcon = (tex, res, name) =>
        {
            var rt = new RenderTexture(res.x, res.y, 32);

            Graphics.SetRenderTarget(rt);
            GL.LoadPixelMatrix(0.0f, 1.0f, 1.0f, 0.0f);
            GL.Clear(true, true, Color.clear);
            Graphics.DrawTexture(new Rect(0.0f, 0.0f, 1.0f, 1.0f), tex);

            Texture2D output = new Texture2D(res.x, res.y);
            output.ReadPixels(new Rect(0.0f, 0.0f, res.x, res.y), 0, 0, false);
            output.Apply();
            rt.Release();

            File.WriteAllBytes(Application.dataPath + path + name + ".png", output.EncodeToPNG());
        };

        foreach (var res in compositeIcons)
        {
            resizeIcon(iconBG, res, $"Icon BG {res.x}x{res.y}");
            resizeIcon(iconFG, res, $"Icon FG {res.x}x{res.y}");
        }

        Texture2D comp = new Texture2D(iconBG.width, iconBG.height);
        for (int x = 0; x < comp.width; x++)
        {
            for (int y = 0; y < comp.height; y++)
            {
                Color bg = iconBG.GetPixel(x, y);
                Color fg = iconFG.GetPixel(x, y);
                comp.SetPixel(x, y, Color.Lerp(bg, fg, fg.a));
            }
        }
        comp.Apply();

        foreach (var res in staticIcons)
        {
            resizeIcon(comp, res, $"Icon Comp {res.x}x{res.y}");
        }

        AssetDatabase.Refresh();
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
