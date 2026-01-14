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
       
        string escena = PlayerPrefs.GetString("UltimaEscena", "Nivel Desconocido");
        float tuTiempo = PlayerPrefs.GetFloat("UltimoTiempo", 0f);
        string nombreUser = PlayerPrefs.GetString("NombreUsuario", "Jugador");

        
        if (txtTituloNivel != null) txtTituloNivel.text = "Tablero: " + escena;
        if (txtTuTiempo != null) txtTuTiempo.text = nombreUser + ": " + tuTiempo.ToString("F2") + "s";

        
        string tabla = ""; 

        for (int i = 1; i <= 3; i++)
        {
            string claveRecord = escena + "_Record_" + i;
            string claveNombre = escena + "_Nombre_" + i;

            float t = PlayerPrefs.GetFloat(claveRecord, 99999f);
            string n = PlayerPrefs.GetString(claveNombre, "---");

            string tiempoTexto = (t >= 99999) ? "---" : t.ToString("F2") + "s";
            tabla += i + ". " + n + " - " + tiempoTexto + "\n";
        }

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