using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] TMP_Dropdown fullscreenDropdown;

    Vector2Int[] resolutions = new Vector2Int[]
    {
        new Vector2Int(1920, 1080),
        new Vector2Int(1280, 720),
        new Vector2Int(2560, 1440),
    };

    private void OnEnable()
    {
        if (resolutionDropdown)
        {
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            foreach (var res in resolutions)
            {
                options.Add($"{res.x}x{res.y}");

            }

            resolutionDropdown.AddOptions(options);
        }

        if (fullscreenDropdown)
        {
            fullscreenDropdown.ClearOptions();
            fullscreenDropdown.AddOptions(new List<string>()
            {
                "Windowed", "Fullscreen", "Borderless"
            });
        }
    }

    public void SetResolution(int i)
    {
        Vector2Int res = resolutions[i];
        Screen.SetResolution(res.x, res.y, Screen.fullScreenMode);

#if UNITY_EDITOR
        print($"Debug changed resolution to {res.x}x{res.y}");
#endif

        using var options = OptionsData.Get();
        options.resolution = res;
    }

    public void SetFullscreenMode (int i)
    {
        FullScreenMode mode;
        switch (i)
        {
            case 0:
                mode = FullScreenMode.Windowed;
                break;
            case 1:
                mode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 2:
                mode = FullScreenMode.FullScreenWindow;
                break;
            default:
                return;
        }

#if UNITY_EDITOR
        Debug.Log($"Changed screen mode to \"{mode}\"");
#endif

        Screen.SetResolution(Screen.width, Screen.height, mode);
    }
}
