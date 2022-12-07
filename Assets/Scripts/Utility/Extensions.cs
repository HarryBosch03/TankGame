using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    public static Vector2 ClosestPoint (this Rect rect, Vector2 point)
    {
        return new Vector2(Mathf.Clamp(point.x, rect.xMin, rect.xMax), Mathf.Clamp(point.y, rect.yMin, rect.yMax));
    }

    public static T Evaluate<T>(this IEnumerable<WeightedElement<T>> list, float selWeight)
    {
        float totalWeight = 0.0f;
        foreach (var element in list)
        {
            totalWeight += element.weight;
        }

        T last = default;
        foreach (var element in list)
        {
            last = element.element;

            if (selWeight <= element.weight)
            {
                return element.element;
            }
            selWeight -= element.weight;
        }

        return last;
    }
}
