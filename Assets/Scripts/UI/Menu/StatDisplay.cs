using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class StatDisplay : MonoBehaviour
{
    public TMPro.TMP_Text textElement;
    public string key;
    public string textTemplate;

    Stats.Stat stat;

    private void OnEnable()
    {
        stat = typeof(Stats).GetField(key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase).GetValue(Stats.Main) as Stats.Stat;
        stat.ValueChangedEvent += OnValueChangedEvent;

        OnValueChangedEvent();
    }

    private void OnDisable()
    {
        stat.ValueChangedEvent -= OnValueChangedEvent;
    }

    private void OnValueChangedEvent()
    {
        textElement.text = Format(stat);
    }

    protected virtual string Format (object value)
    {
        return string.Format(textTemplate, value.ToString());
    }
}
