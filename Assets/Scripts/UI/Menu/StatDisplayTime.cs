using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays a float stat as a time.
/// </summary>
public class StatDisplayTime : StatDisplay
{
    protected override string Format(object value)
    {
        var timespan = TimeSpan.FromSeconds((float)value);
        return timespan.ToString(textTemplate);
    }
}
