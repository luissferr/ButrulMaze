using UnityEngine;

public class SoundManager:MonoBehaviour
{
    public static SoundManager instance;

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

    public void CambiarVolumen(float v)
    {
        audioSource.volume = v;
        PlayerPrefs.SetFloat("VolSFX", v);
    }

    public void Boton()
    {
        if (clickBoton != null)
            audioSource.PlayOneShot(clickBoton);
    }
}
