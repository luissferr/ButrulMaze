using UnityEngine;

public class PlayerColor:MonoBehaviour
{
    void Start()
    {
        if (!PlayerPrefs.HasKey("SkinColor")) return;

        string hex = PlayerPrefs.GetString("SkinColor");
        Color c;
        if (ColorUtility.TryParseHtmlString(hex, out c))
        {
            var r = GetComponent<Renderer>();
            if (r) r.material.color = c;
        }
    }
}
