using System.Collections.Generic;
using UnityEngine;

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
