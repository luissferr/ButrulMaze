using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RankingManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public Text txtTituloNivel;
    public Text txtTiemposTop;
    public Text txtTuTiempo;

    void Start()
    {
        // 1. Recuperamos los datos guardados
        string escena = PlayerPrefs.GetString("UltimaEscena", "Nivel Desconocido");
        float tuTiempo = PlayerPrefs.GetFloat("UltimoTiempo", 0f);
        string nombreUser = PlayerPrefs.GetString("NombreUsuario", "Jugador");

        // 2. Asignamos textos principales
        if (txtTituloNivel != null) txtTituloNivel.text = "Tablero: " + escena;
        if (txtTuTiempo != null) txtTuTiempo.text = nombreUser + ": " + tuTiempo.ToString("F2") + "s";

        // 3. Construimos la tabla de posiciones
        string tabla = ""; // Declaramos la variable AQUÍ para que exista en todo el método

        for (int i = 1; i <= 3; i++)
        {
            string claveRecord = escena + "_Record_" + i;
            string claveNombre = escena + "_Nombre_" + i;

            float t = PlayerPrefs.GetFloat(claveRecord, 999.9f);
            string n = PlayerPrefs.GetString(claveNombre, "---");

            string tiempoTexto = (t >= 999) ? "---" : t.ToString("F2") + "s";
            tabla += i + ". " + n + " - " + tiempoTexto + "\n";
        }

        // 4. Asignamos la tabla al texto de la UI
        if (txtTiemposTop != null)
        {
            txtTiemposTop.text = tabla;
        }
    }

    public void Reintentar()
    {
        string ultimaEscena = PlayerPrefs.GetString("UltimaEscena", "");
        if (!string.IsNullOrEmpty(ultimaEscena))
        {
            SceneManager.LoadScene(ultimaEscena);
        }
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}