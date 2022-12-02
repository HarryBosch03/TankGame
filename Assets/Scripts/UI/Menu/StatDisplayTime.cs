using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatDisplayTime : StatDisplay
{
    protected override string Format(object value)
    {
        var timespan = TimeSpan.FromSeconds((float)value);
        return timespan.ToString(textTemplate);
    }
}
