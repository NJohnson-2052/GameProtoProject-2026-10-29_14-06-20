
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Player player;

    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Fire1 pressed");

            if (Time.time < nextFireTime)
            {
                Debug.Log("Still on cooldown");
                return;
            }

            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null) { Debug.LogError("Weapon: Bullet Prefab slot is empty"); return; }
        if (firePoint == null)    { Debug.LogError("Weapon: Fire Point slot is empty"); return; }
        if (player == null)       { Debug.LogError("Weapon: Player slot is empty"); return; }

        WeaponData data = player.CurrentWeaponData;
        if (data == null)
        {
            Debug.LogError("Weapon: Player's Weapons array is empty or has a None element");
            return;
        }

        nextFireTime = Time.time + data.fireRate;
        Debug.Log("Shooting with " + data.weaponName);

        for (int i = 0; i < data.bulletCount; i++)
        {
            float angle = 0f;
            if (data.bulletCount > 1)
            {
                float t = (float)i / (data.bulletCount - 1);
                angle = Mathf.Lerp(-data.spreadAngle, data.spreadAngle, t);
            }

            Quaternion rot = firePoint.rotation * Quaternion.Euler(0, 0, angle);
            GameObject obj = Instantiate(bulletPrefab, firePoint.position, rot);

            Bullet b = obj.GetComponent<Bullet>();
            if (b != null) b.Setup(data, player.currentWeapon);
            else Debug.LogError("Weapon: Bullet prefab has no Bullet script on it");
        }
    }
}