using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName = "Buster";
    public Color color = Color.cyan;
    public int damage = 1;
    public float fireRate = 0.3f;     // seconds between shots
    public int bulletCount = 1;       // 3 = spread shot
    public float spreadAngle = 15f;
    public bool pierce = false;       // passes through enemies
}