using UnityEngine;

public class WeaponHUD : MonoBehaviour
{
    public Player player;
    public float x = 15f;
    public float y = 15f;
    public float labelWidth = 100f;
    public float barWidth = 120f;
    public float barHeight = 16f;
    public float spacing = 26f;

    private GUIStyle label;

    void OnGUI()
    {
        if (player == null || player.weapons == null || player.energy == null) return;

        if (label == null)
        {
            label = new GUIStyle(GUI.skin.label);
            label.fontSize = 13;
            label.fontStyle = FontStyle.Bold;
            label.normal.textColor = Color.white;
        }

        for (int i = 0; i < player.weapons.Length; i++)
        {
            WeaponData w = player.weapons[i];
            if (w == null || i >= player.energy.Length) continue;

            float rowY = y + i * spacing;
            bool selected = (i == player.currentWeapon);
            bool infinite = w.energyCost <= 0;
            float pct = infinite ? 1f : Mathf.Clamp01((float)player.energy[i] / w.maxEnergy);

            // Highlight the equipped weapon's row
            if (selected)
            {
                GUI.color = new Color(1f, 1f, 1f, 0.3f);
                GUI.DrawTexture(new Rect(x - 4, rowY - 3, labelWidth + barWidth + 12, barHeight + 6), Texture2D.whiteTexture);
            }

            // Name
            GUI.color = Color.white;
            GUI.Label(new Rect(x, rowY - 3, labelWidth, barHeight + 6), (i + 1) + " " + w.weaponName, label);

            float barX = x + labelWidth;

            // Border, empty, filled (filled uses the weapon's own color)
            GUI.color = Color.black;
            GUI.DrawTexture(new Rect(barX - 2, rowY - 2, barWidth + 4, barHeight + 4), Texture2D.whiteTexture);
            GUI.color = new Color(0.2f, 0.2f, 0.2f);
            GUI.DrawTexture(new Rect(barX, rowY, barWidth, barHeight), Texture2D.whiteTexture);
            GUI.color = w.color;
            GUI.DrawTexture(new Rect(barX, rowY, barWidth * pct, barHeight), Texture2D.whiteTexture);

            // Numbers on the bar
            GUI.color = Color.white;
            string text = infinite ? "INF" : player.energy[i] + "/" + w.maxEnergy;
            GUI.Label(new Rect(barX + 4, rowY - 3, barWidth, barHeight + 6), text, label);
        }

        GUI.color = Color.white;
    }
}