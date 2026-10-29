using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public WeaponData[] weapons;      // Fill in the Inspector
    public int currentWeapon = 0;

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

        // Q cycles forward
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetWeapon((currentWeapon + 1) % weapons.Length);
        }

        // Number keys 1-9 select directly
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

    // Mega Man-style: player tints to match the active weapon
    void ApplyWeaponColor()
    {
        if (sr != null && CurrentWeaponData != null)
        {
            sr.color = CurrentWeaponData.color;
        }
    }
}