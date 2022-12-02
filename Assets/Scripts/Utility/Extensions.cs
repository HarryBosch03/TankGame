using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static Vector2 ClosestPoint (this Rect rect, Vector2 point)
    {
        return new Vector2(Mathf.Clamp(point.x, rect.xMin, rect.xMax), Mathf.Clamp(point.y, rect.yMin, rect.yMax));
    }

    public static T Evaluate<T>(this IList<WeightedElement<T>> list)
    {
        float totalWeight = 0.0f;
        foreach (var element in list)
        {
            totalWeight += element.weight;
        }

        float selectWeight = Random.value * totalWeight;
        foreach (var element in list)
        {
            if (selectWeight <= element.weight)
            {
                return element.element;
            }
            selectWeight -= element.weight;
        }

        return list[list.Count - 1].element;
    }
}
