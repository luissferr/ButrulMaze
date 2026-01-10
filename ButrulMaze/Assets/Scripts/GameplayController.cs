using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayController:MonoBehaviour
{
    public static GameplayController instance;
    public GameObject bola;
    public Text txtCrono, txtGemas;
    public GameObject panelPausa;

    private float tiempo;
    public int recolectadas = 0, totales;
    private Vector3 spawn;
    private bool activo = true;

    void Awake() => instance = this;

    void Start()
    {
        panelPausa.SetActive(false); // aseguramos que empieza oculto
        activo = true;               // para Update

        spawn = bola.transform.position;
        Time.timeScale = 1f;
        totales = GameObject.FindGameObjectsWithTag("Gema").Length;
        ActualizarInterfaz();

        Color c;
        if (ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("SkinColor", "#FFFFFF"), out c))
            bola.GetComponent<Renderer>().material.color = c;
    }

    void Update()
    {
        if (!activo) return; //

        // Solo actuamos si la bola cae por debajo del nivel del suelo (ejemplo: -1.0)
        if (bola.transform.position.y < -5.0f)
        {
            // En lugar de reiniciar, la "rescatamos" poniéndola un poco arriba
            Vector3 posicionRescate = bola.transform.position;
            posicionRescate.y = 1.0f; // La ponemos a una altura segura sobre el tablero
            bola.transform.position = posicionRescate;

            // Frenamos su velocidad de caída para que no rebote como loca
            Rigidbody rb = bola.GetComponent<Rigidbody>();
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        }

        // El resto de tu código de tiempo y gemas...
        tiempo += Time.deltaTime; //
        txtCrono.text = tiempo.ToString("F2") + "s"; //
    }

    public void RecogerGema() {
        recolectadas++;
        ActualizarInterfaz();

    }
    void ActualizarInterfaz() => txtGemas.text = "Gemas: " + recolectadas + "/" + totales;

    public void IntentarGanar()
    {
        if (recolectadas >= totales)
        {
            activo = false;
            PlayerPrefs.SetFloat("UltimoTiempo", tiempo);
            SceneManager.LoadScene("Ranking");
        }
    }

    public void Reiniciar()
    {
        Rigidbody rb = bola.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero; bola.transform.position = spawn;
    }

    public void Pausar(bool p) { panelPausa.SetActive(p); Time.timeScale = p ? 0f : 1f; }
    public void Menu()
    {
        Time.timeScale = 1f; // <-- IMPORTANTE: reactiva el tiempo
        SceneManager.LoadScene("Menu");
    }
}