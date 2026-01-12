using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Clips")]
    public AudioClip clickBoton;
    public AudioClip choque;
    public AudioClip gema;
    public AudioClip meta;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        float vol = PlayerPrefs.GetFloat("VolSFX", 0.7f);
        audioSource.volume = vol;
    }

    // --- Volumen dinámico ---
    public void CambiarVolumen(float v)
    {
        audioSource.volume = v;
        PlayerPrefs.SetFloat("VolSFX", v);
    }

    // --- Métodos para reproducir cada sonido ---
    public void Boton()
    {
        if (clickBoton != null)
            audioSource.PlayOneShot(clickBoton);
    }

    public void Choque()
    {
        if (choque != null)
            audioSource.PlayOneShot(choque);
    }

    public void Gema()
    {
        if (gema != null)
            audioSource.PlayOneShot(gema);
    }

    public void Meta()
    {
        if (meta != null)
            audioSource.PlayOneShot(meta);
    }
}