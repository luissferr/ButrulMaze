using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();

            float volGuardado = PlayerPrefs.GetFloat("VolMusica", 0.7f);
            if (audioSource != null)
            {
                audioSource.volume = volGuardado;
                if (!audioSource.isPlaying) audioSource.Play();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CambiarVolumen(float v)
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (audioSource != null) audioSource.volume = v;

       
        PlayerPrefs.SetFloat("VolMusica", v);
        PlayerPrefs.Save();
    }
}