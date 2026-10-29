using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health = 3;
    public int weakness = 1;
    public float deathDuration = 0.4f;

    [Header("Health Bar")]
    public bool showHealthBar = true;
    public float barWidth = 80f;     // pixels
    public float barHeight = 10f;    // pixels
    public float barOffset = 15f;    // pixels above the enemy

    private int maxHealth;
    private SpriteRenderer sr;
    private Color originalColor;
    private bool isDying = false;

    void Awake()
    {
        maxHealth = health;   // whatever you set in the Inspector is full health

        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            originalColor = sr.color;
        }
    }

    public void TakeDamage(int weaponType, int damage)
    {
        if (isDying) return;

        bool isCrit = (weaponType == weakness);
        if (isCrit)
        {
            damage *= 3;
        }

        health -= damage;
        Debug.Log(name + " health: " + health + (isCrit ? " (CRIT!)" : ""));

        if (health <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(Die());
        }
        else if (sr != null)
        {
            StopAllCoroutines();
            StartCoroutine(FlashColor(isCrit ? Color.white : Color.red));
        }
    }

    IEnumerator FlashColor(Color flashColor)
    {
        sr.color = flashColor;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }

    IEnumerator Die()
    {
        isDying = true;

        foreach (Collider2D c in GetComponents<Collider2D>())
        {
            c.enabled = false;
        }

        if (sr == null)
        {
            Destroy(gameObject);
            yield break;
        }

        Vector3 startScale = transform.localScale;
        float timer = 0f;

        while (timer < deathDuration)
        {
            timer += Time.deltaTime;
            float t = timer / deathDuration;

            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            transform.Rotate(0, 0, 720f * Time.deltaTime);

            Color c = Color.white;
            c.a = 1f - t;
            sr.color = c;

            yield return null;
        }

        Destroy(gameObject);
    }

    // Draws a simple health bar above the enemy
    void OnGUI()
    {
        if (!showHealthBar || isDying || sr == null || Camera.main == null) return;

        // Top-center of the sprite in world space
        Vector3 top = new Vector3(sr.bounds.center.x, sr.bounds.max.y, 0f);
        Vector3 screen = Camera.main.WorldToScreenPoint(top);

        if (screen.z < 0) return;   // behind the camera

        // OnGUI's Y axis is flipped compared to screen space
        float x = screen.x - barWidth / 2f;
        float y = Screen.height - screen.y - barOffset - barHeight;

        float percent = Mathf.Clamp01((float)health / maxHealth);

        // Background
        GUI.color = Color.black;
        GUI.DrawTexture(new Rect(x - 2, y - 2, barWidth + 4, barHeight + 4), Texture2D.whiteTexture);

        // Empty (dark red)
        GUI.color = new Color(0.4f, 0f, 0f);
        GUI.DrawTexture(new Rect(x, y, barWidth, barHeight), Texture2D.whiteTexture);

        // Filled (green)
        GUI.color = Color.green;
        GUI.DrawTexture(new Rect(x, y, barWidth * percent, barHeight), Texture2D.whiteTexture);

        GUI.color = Color.white;   // reset so other GUI isn't tinted
    }
}