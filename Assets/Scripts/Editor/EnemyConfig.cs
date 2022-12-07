using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

public class EnemyConfig : EditorWindow
{
    [MenuItem("Window/Tools/Enemy Config")]
    public static void Open ()
    {
        GetWindow<EnemyConfig>("Enemy Config");
    }

    EnemySpawnProfile target;
    Editor editor;

    float timeSlice;

    private void OnGUI()
    {
        List<EnemySpawnProfile> profiles = GetAllEnemyProfiles();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
        foreach (var profile in profiles)
        {
            if (GUILayout.Button(profile.name))
            {
                target = profile;
            }
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Deselect"))
        {
            target = null;
        }

        if (GUILayout.Button("Rename Profiles"))
        {
            foreach (var profile in profiles)
            {
                profile.name = profile.Prefab.name + " Profile";
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(profile), profile.name);
            }

            AssetDatabase.Refresh();
        }

        if (GUILayout.Button("Create Missing Profiles"))
        {
            CreateMissingProfiles();
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        if (target)
        {
            GUILayout.Label(target.name);
            EditorGUILayout.Space();

            Editor.CreateCachedEditor(target, null, ref editor);
            editor.OnInspectorGUI();

            EditorGUILayout.Space();

            if (GUILayout.Button("Select Profile"))
            {
                Selection.activeObject = target;
                ProjectWindowUtil.ShowCreatedAsset(target);
            }
        }
        else
        {
            float maxTime = 0.0f;
            foreach (var profile in profiles)
            {
                maxTime = Mathf.Max(maxTime, profile.MinTime);
            }

            timeSlice = EditorGUILayout.Slider("Time Slice", timeSlice, 0.0f, maxTime);

            var rect = EditorGUILayout.GetControlRect(GUILayout.ExpandHeight(true));
            var validProfiles = profiles.Where(p => p.MinTime <= timeSlice).OrderBy(p => p.MinTime).ToArray();

            float totalWeight = 0.0f;
            foreach (var profile in validProfiles)
            {
                totalWeight += profile.Weight;
            }

            float offset = 0.0f;
            foreach (var profile in validProfiles)
            {
                float percent = profile.Weight / totalWeight;
                float height = percent * rect.height;

                var col = Color.HSVToRGB(Mathf.Abs(profile.name.GetHashCode() / 1000.0f) % 1.0f, 0.8f, 0.9f);
                Rect localRect = new Rect(rect.x, rect.y + offset, rect.width, height);
                EditorGUI.DrawRect(localRect, col);
                if (percent > 0.01f) EditorGUI.LabelField(localRect, profile.name, new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, alignment = TextAnchor.MiddleCenter });

                offset += height;
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    private static List<EnemySpawnProfile> GetAllEnemyProfiles()
    {
        var profilePaths = AssetDatabase.FindAssets($"t:{nameof(EnemySpawnProfile)}", new string[] { "Assets" });
        var profiles = new List<EnemySpawnProfile>();
        foreach (var path in profilePaths)
        {
            profiles.Add(AssetDatabase.LoadAssetAtPath<EnemySpawnProfile>(AssetDatabase.GUIDToAssetPath(path)));
        }

        return profiles;
    }

    private void CreateMissingProfiles()
    {
        List<GameObject> enemyPrefabs = new List<GameObject>();

        foreach (var asset in AssetDatabase.GetAllAssetPaths())
        {
            if (asset.Length < 7) continue;
            if (asset.Substring(asset.Length - 7, 7) != ".prefab") continue;

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(asset);

            if (prefab.TryGetComponent(out EnemyBase _))
            {
                enemyPrefabs.Add(prefab);
            }
        }

        var enemyProfiles = GetAllEnemyProfiles();
        HashSet<GameObject> existingProfiles = new HashSet<GameObject>();

        foreach (var profile in enemyProfiles)
        {
            existingProfiles.Add(profile.Prefab);
        }

        foreach (var enemy in enemyPrefabs)
        {
            if (existingProfiles.Contains(enemy)) continue;

            var profile = CreateInstance<EnemySpawnProfile>();
            profile.Prefab = enemy;
            profile.Weight = 1.0f;
            profile.MinTime = 0.0f;

            AssetDatabase.CreateAsset(profile, $"Assets/Scriptable Objects/Enemy Spawn Profiles/{enemy.name} Profile.asset");
        }
    }
}
