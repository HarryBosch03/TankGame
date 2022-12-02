using UnityEngine;

public abstract class Crate : MonoBehaviour
{
    public Team[] validTeams;

    public GameObject pickupFX;

    private void Start()
    {
        pickupFX.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out TeamPlayer teamPlayer)) return;
        if (!Team.IsInOne(teamPlayer, validTeams)) return;

        if (TryApplyEffect(collision.gameObject))
        {
            pickupFX.SetActive(true);
            pickupFX.transform.SetParent(null);

            Destroy(gameObject);
        }
        
    }

    protected abstract bool TryApplyEffect(GameObject target);
}
