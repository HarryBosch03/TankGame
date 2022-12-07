using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wrapper class for weighed arrays, uses extension methids to evaluate.
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class WeightedElement<T>
{
    public T element;
    public float weight;

    public WeightedElement(T element, float weight)
    {
        this.element = element;
        this.weight = weight;
    }
}
