using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// Returns the closest point on a rect, its a joke that Unity has a funtion like this for the bounds struct and not the rect one. #FuckUnity
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public static Vector2 ClosestPoint (this Rect rect, Vector2 point)
    {
        return new Vector2(Mathf.Clamp(point.x, rect.xMin, rect.xMax), Mathf.Clamp(point.y, rect.yMin, rect.yMax));
    }

    /// <summary>
    /// Extension for evaluating weighted arrays (arrays containing a weightedElement object)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="selWeight"></param>
    /// <returns></returns>
    public static T Evaluate<T>(this IEnumerable<WeightedElement<T>> list, float selWeight)
    {
        float totalWeight = 0.0f;
        foreach (var element in list)
        {
            totalWeight += element.weight;
        }

        selWeight *= totalWeight;

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

    /// <summary>
    /// Extenstion for evaluating arrays of object that contain a weight within the class without it being of type WeightedElement
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="getWeight"></param>
    /// <param name="selWeight"></param>
    /// <returns></returns>
    public static T GetWeightedElement<T>(this IEnumerable<T> list, System.Func<T, float> getWeight, float selWeight)
    {
        float totalWeight = 0.0f;
        foreach (var element in list)
        {
            totalWeight += getWeight(element);
        }

        selWeight *= totalWeight;

        T last = default;
        foreach (var element in list)
        {
            last = element;

            if (selWeight <= getWeight(element))
            {
                return element;
            }
            selWeight -= getWeight(element);
        }

        return last;
    }

    public static void ExecuteDelayed (this MonoBehaviour ctx, System.Action callback, float delaySecconds)
    {
        ctx.StartCoroutine(ExecuteDelayedRoutine(callback, delaySecconds));
    }

    static IEnumerator ExecuteDelayedRoutine (System.Action callback, float delaySecconds)
    {
        yield return new WaitForSeconds(delaySecconds);
        callback?.Invoke();
    }
}
