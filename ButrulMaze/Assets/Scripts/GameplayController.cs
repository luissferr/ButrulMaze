using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        Time.timeScale = 1f; // Aseguramos que el tiempo corre

        totales = GameObject.FindGameObjectsWithTag("Gema").Length;
        ActualizarInterfaz();

        Color c;
        if (ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("SkinColor", "#FFFFFF"), out c))
            bola.GetComponent<Renderer>().material.color = c;
    }

    void Update()
    {
        if (!activo) return;
        if (bola.transform.position.y < -15.0f) ReiniciarPosicion();
        tiempo += Time.deltaTime;
        txtCrono.text = tiempo.ToString("F2") + "s";
    }

    public void Pausa(bool pausar)
    {
        activo = !pausar;
        panelPausa.SetActive(pausar);
        Time.timeScale = pausar ? 0f : 1f;
    }

    public void Menu()
    {
        Time.timeScale = 1f; // Descongelamos antes de irnos
        SceneManager.LoadScene("Menu");
    }

    public void ReiniciarPosicion()
    {
        bola.transform.position = spawn + Vector3.up;
        Rigidbody rb = bola.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void RecogerGema() { recolectadas++; ActualizarInterfaz(); }
    void ActualizarInterfaz() => txtGemas.text = "Gemas: " + recolectadas + "/" + totales;

    public void IntentarGanar()
    {
        if (recolectadas >= totales)
        {
            activo = false;
            PlayerPrefs.SetFloat("UltimoTiempo", tiempo);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Ranking"); // O donde debas ir
        }
    }

    public void RegistrarVictoria()
    {
        activo = false;
        string nombreEscena = SceneManager.GetActiveScene().name;
        float tiempoFinal = tiempo;
        string nombreJugador = PlayerPrefs.GetString("NombreUsuario", "Jugador");

        // Guardamos los datos de la partida actual para la UI de esta sesión
        PlayerPrefs.SetFloat("UltimoTiempo", tiempoFinal);
        PlayerPrefs.SetString("UltimaEscena", nombreEscena);

        // --- LÓGICA DE RANKING TOP 3 PERSISTENTE ---
        for (int i = 1; i <= 3; i++)
        {
            string claveRecord = nombreEscena + "_Record_" + i;
            string claveNombre = nombreEscena + "_Nombre_" + i;

            // Usamos un valor muy alto para que cualquier tiempo entre la primera vez
            float recordGuardado = PlayerPrefs.GetFloat(claveRecord, 99999f);

            if (tiempoFinal < recordGuardado)
            {
                // DESPLAZAMIENTO: Movemos los puestos de abajo hacia el final
                // Si entramos en el puesto 1, el 2 pasa al 3, y el 1 pasa al 2.
                for (int j = 3; j > i; j--)
                {
                    string anteriorRecord = nombreEscena + "_Record_" + (j - 1);
                    string anteriorNombre = nombreEscena + "_Nombre_" + (j - 1);

                    PlayerPrefs.SetFloat(nombreEscena + "_Record_" + j, PlayerPrefs.GetFloat(anteriorRecord, 99999f));
                    PlayerPrefs.SetString(nombreEscena + "_Nombre_" + j, PlayerPrefs.GetString(anteriorNombre, "---"));
                }

                // INSERTAR: Guardamos el nuevo récord en la posición i
                PlayerPrefs.SetFloat(claveRecord, tiempoFinal);
                PlayerPrefs.SetString(claveNombre, nombreJugador);

                break; // Salimos del bucle tras insertar el récord
            }
        }
        PlayerPrefs.Save(); // Forzamos la persistencia en el disco
    }
}