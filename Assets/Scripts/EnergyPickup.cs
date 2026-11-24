using UnityEngine;

public class EnergyPickup : MonoBehaviour
{
    public int amount = 5;
    public float lifetime = 10f;   // disappears if not collected

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Stay (not Enter) so a pickup touched while energy is full can still be grabbed later
    void OnTriggerStay2D(Collider2D other)
    {
        Player p = other.GetComponentInParent<Player>();
        if (p == null) return;

        if (p.AddEnergy(amount))
        {
            Destroy(gameObject);
        }
    }
}