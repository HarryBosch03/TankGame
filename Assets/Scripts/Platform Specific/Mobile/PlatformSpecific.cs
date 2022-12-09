using UnityEngine;

public class PlatformSpecific : MonoBehaviour
{
    [SerializeField] bool pc;
    [SerializeField] bool mobile;
    [SerializeField] bool web;

    private void OnEnable()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.LinuxPlayer:
            case RuntimePlatform.LinuxEditor:
                gameObject.SetActive(pc);
                break;

            case RuntimePlatform.IPhonePlayer:
            case RuntimePlatform.Android:
                gameObject.SetActive(mobile);
                break;

            case RuntimePlatform.WebGLPlayer:
                gameObject.SetActive(web);
                break;
        }
    }
}
