using UnityEngine;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;

    private int damage = 1;
    private int weaponIndex = 0;
    private bool pierce = false;
    private HashSet<Enemy> alreadyHit = new HashSet<Enemy>();

    // Called by Weapon right after Instantiate
    public void Setup(WeaponData data, int index)
    {
        damage = data.damage;
        weaponIndex = index;
        pierce = data.pierce;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = data.color;
    }

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;   // Unity 6 would be rb.linearVelocity
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.GetComponentInParent<Player>() != null) return;

        Enemy enemy = hitInfo.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            // Guard against hitting both of an enemy's colliders
            if (!alreadyHit.Add(enemy)) return;

            enemy.TakeDamage(weaponIndex, damage);

            if (!pierce) Destroy(gameObject);
            return;
        }

        // Hit a wall/floor (ignore other triggers)
        if (!hitInfo.isTrigger) Destroy(gameObject);
    }
}