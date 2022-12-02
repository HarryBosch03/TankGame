using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects/Signal")]
public class Signal : ScriptableObject
{
    [SerializeField] bool debug;

    public event System.Action OnRaise;

    public void Raise ()
    {
        if (debug) Debug.Log($"{name} Raised!");
        OnRaise?.Invoke();
    }
}
