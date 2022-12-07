using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// Controller used for displaying generic messages on screen, inspired by minecrafts /title command
/// </summary>
[DisallowMultipleComponent]
public class Toast : MonoBehaviour
{
    public string id;
    public AnimationCurve interpolationCurve;

    TMPro.TMP_Text textElement;

    List<ToastData> toasts;
    Dictionary<ToastData, ToastMeta> toastsMeta;

    private static event System.Action<ToastData> ShowToastEvent;

    public static void DisplayMessage(ToastData data) => ShowToastEvent?.Invoke(data);

    private void Awake()
    {
        textElement = GetComponent<TMPro.TMP_Text>();

        toasts = new List<ToastData>();
        toastsMeta = new Dictionary<ToastData, ToastMeta>();
    }

    private void OnEnable()
    {
        ShowToastEvent += OnShowToastEvent;
    }

    private void OnDisable()
    {
        ShowToastEvent -= OnShowToastEvent;
    }

    private void Update()
    {
        StringBuilder builder = new StringBuilder();

        for (int i = 0; i < toasts.Count;)
        {
            if (toastsMeta[toasts[i]].age > toasts[i].TotalTime)
            {
                toastsMeta.Remove(toasts[i]);
                toasts.RemoveAt(i);
            }
            else i++;
        }

        foreach (ToastData toast in toasts)
        {
            ToastMeta meta = toastsMeta[toast];
            float fade = Mathf.Clamp01((meta.age < toast.TotalTime / 2.0f) ? meta.age / toast.fadeInTime : (toast.TotalTime - meta.age) / toast.fadeOutTime);
            byte alpha = (byte)(byte.MaxValue * interpolationCurve.Evaluate(fade));

            string hex = ColorUtility.ToHtmlStringRGB(toast.color);
            builder.Append("<color=#").Append(hex).Append(alpha.ToString("X2")).Append(">");
            builder.Append(toast.text);
            builder.Append("\n");

            meta.age += Time.deltaTime;
        }

        textElement.text = builder.ToString();
    }

    private void OnShowToastEvent(ToastData data)
    {
        if (id.ToLower() == data.id.ToLower())
        {
            toasts.Insert(0, data);
            toastsMeta.Add(data, new ToastMeta());
        }
    }

    public class ToastData
    {
        public string id;
        public string text;
        public Color color = Color.white;
        public float duration;
        public float fadeInTime;
        public float fadeOutTime;

        public float TotalTime => duration + fadeInTime + fadeOutTime;

        public ToastData(string id, string text, Color color, float duration = 3.0f, float fadeInTime = 0.1f, float fadeOutTime = 3.0f)
        {
            this.id = id;
            this.text = text;
            this.color = color;
            this.duration = duration;
            this.fadeInTime = fadeInTime;
            this.fadeOutTime = fadeOutTime;
        }
    }

    public class ToastMeta
    {
        public float age;
    }

}
