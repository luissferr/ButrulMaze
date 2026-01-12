using UnityEngine;

public class MusicManager:MonoBehaviour
{
    public static MusicManager instance;
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
        float vol = PlayerPrefs.GetFloat("VolMusica", 0.7f);
        audioSource.volume = vol;
        audioSource.loop = true;

        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void CambiarVolumen(float v)
    {
        audioSource.volume = v;
        PlayerPrefs.SetFloat("VolMusica", v);
    }
}
