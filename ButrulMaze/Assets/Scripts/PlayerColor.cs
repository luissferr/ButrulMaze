using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    void Start()
    {
        
        if (!PlayerPrefs.HasKey("SkinColor")) return;

        
        string hex = PlayerPrefs.GetString("SkinColor");

        Color c;
        
        if (ColorUtility.TryParseHtmlString(hex, out c))
        {
            
            Renderer r = GetComponent<Renderer>();

            if (r != null)
            {
                
                r.material.color = c;
            }
        }
    }
}