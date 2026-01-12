using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour
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
        panelPausa.SetActive(false);
        activo = true;
        spawn = bola.transform.position;
        Time.timeScale = 1f;
        totales = GameObject.FindGameObjectsWithTag("Gema").Length;
        ActualizarInterfaz();

        // Aplicar el color guardado en el menú a la bola real
        Color c;
        if (ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("SkinColor", "#FFFFFF"), out c))
            bola.GetComponent<Renderer>().material.color = c;
    }

    void Update()
    {
        if (!activo) return;

        // --- RESCATE SEGURO ---
        // Ponemos -15.0f para dar margen total a la inclinación del tablero
        if (bola.transform.position.y < -15.0f)
        {
            ReiniciarPosicion();
        }

        tiempo += Time.deltaTime;
        txtCrono.text = tiempo.ToString("F2") + "s";
    }

    public void ReiniciarPosicion()
    {
        bola.transform.position = spawn + Vector3.up; // Aparece un poco arriba del spawn
        Rigidbody rb = bola.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero; // Frenamos la bola al rescatarla
        rb.angularVelocity = Vector3.zero;
    }

    public void RecogerGema()
    {
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

    public void Pausar(bool p) { panelPausa.SetActive(p); Time.timeScale = p ? 0f : 1f; }
    public void Menu() { Time.timeScale = 1f; SceneManager.LoadScene("Menu"); }
}