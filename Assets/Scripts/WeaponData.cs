using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName = "Buster";
    public Color color = Color.cyan;
    public int damage = 1;
    public float fireRate = 0.3f;
    public int bulletCount = 1;
    public float spreadAngle = 15f;
    public bool pierce = false;

    [Header("Energy")]
    public int maxEnergy = 20;
    public int energyCost = 1;     // per shot. 0 = infinite (Buster)
}