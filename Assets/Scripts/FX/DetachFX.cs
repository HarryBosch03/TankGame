using UnityEngine;

public class DetachFX : MonoBehaviour
{
    [SerializeField] float delay;

    private void OnTransformParentChanged()
    {
        if (!transform.parent)
        {
            Destroy(gameObject, delay);
        }
    }
}
