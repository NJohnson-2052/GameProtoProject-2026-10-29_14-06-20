using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public WeaponData[] weapons;
    public int currentWeapon = 0;

    [HideInInspector] public int[] energy;   // current energy per weapon

    public WeaponData CurrentWeaponData
    {
        get
        {
            if (weapons == null || weapons.Length == 0) return null;
            return weapons[currentWeapon];
        }
    }

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // Every weapon starts full
        energy = new int[weapons.Length];
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null) energy[i] = weapons[i].maxEnergy;
        }

        ApplyWeaponColor();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(h, 0, 0) * moveSpeed * Time.deltaTime;

        if (h > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (h < 0) transform.rotation = Quaternion.Euler(0, 180, 0);

        HandleWeaponSwap();
    }

    void HandleWeaponSwap()
    {
        if (weapons == null || weapons.Length == 0) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetWeapon((currentWeapon + 1) % weapons.Length);
        }

        for (int i = 0; i < weapons.Length && i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SetWeapon(i);
            }
        }
    }

    void SetWeapon(int index)
    {
        currentWeapon = index;
        ApplyWeaponColor();
        Debug.Log("Weapon: " + CurrentWeaponData.weaponName);
    }

    void ApplyWeaponColor()
    {
        if (sr != null && CurrentWeaponData != null)
        {
            sr.color = CurrentWeaponData.color;
        }
    }

    // ---------- Energy ----------

    public bool CanFire()
    {
        WeaponData d = CurrentWeaponData;
        if (d == null || energy == null) return false;
        return d.energyCost <= 0 || energy[currentWeapon] >= d.energyCost;
    }

    public void SpendEnergy()
    {
        WeaponData d = CurrentWeaponData;
        if (d == null || d.energyCost <= 0) return;
        energy[currentWeapon] = Mathf.Max(0, energy[currentWeapon] - d.energyCost);
    }

    // Returns true if any energy was actually restored
    public bool AddEnergy(int amount)
    {
        if (energy == null) return false;

        int target = currentWeapon;

        // If the equipped weapon is the Buster or already full, refill the emptiest other weapon
        if (weapons[target].energyCost <= 0 || energy[target] >= weapons[target].maxEnergy)
        {
            target = -1;
            float lowest = 1f;
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i] == null || weapons[i].energyCost <= 0) continue;
                float pct = (float)energy[i] / weapons[i].maxEnergy;
                if (pct < lowest)
                {
                    lowest = pct;
                    target = i;
                }
            }
            if (target == -1) return false;   // everything is full
        }

        energy[target] = Mathf.Min(energy[target] + amount, weapons[target].maxEnergy);
        return true;
    }
}