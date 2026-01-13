using UnityEngine;
using UnityEngine.UI;

public class BrightnessManager : MonoBehaviour
{
    public static BrightnessManager instance;
    public Image filtro;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        AplicarBrilloGuardado();
    }

    public void AplicarBrillo(float valor)
    {
        if (filtro == null) return;

        Color c = filtro.color;
        c.a = 1f - valor;
        filtro.color = c;

        PlayerPrefs.SetFloat("Brillo", valor);
    }

    void AplicarBrilloGuardado()
    {
        float v = PlayerPrefs.GetFloat("Brillo", 1f);
        AplicarBrillo(v);
    }
}
