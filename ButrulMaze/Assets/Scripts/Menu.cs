using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject pPortada, pRegistro, pInicio, pConfigPartida, pOpciones, pCreditos, pTutorial;

    [Header("Usuario")]
    public InputField inputNombre;
    public Text txtBienvenida;

    [Header("Opciones")]
    public Slider sliderMusica, sliderBrillo;
    public Image filtroBrillo;

    [Header("Skin (color)")]
    public Image previewColor;
    public Text textoSkin;
    public string[] coloresHex = {
        "#FF0000", // Rojo
        "#00FF00", // Verde
        "#0000FF", // Azul
        "#FFFF00", // Amarillo
        "#FF00FF", // Magenta
        "#00FFFF"  // Cian
    };

    int colorIndex = 0;


    void Start()
    {
        CargarAjustes();
        CargarSkin();

        // Si ya existe un nombre, vamos directo al menú principal (pInicio)
        if (PlayerPrefs.HasKey("NombreUsuario"))
        {
            inputNombre.text = PlayerPrefs.GetString("NombreUsuario");
            txtBienvenida.text = "Hola, " + PlayerPrefs.GetString("NombreUsuario");
            ActivarPanel(pInicio); // <-- Esto salta la portada
        }
        else
        {
            ActivarPanel(pPortada); // <-- Primera vez: muestra portada
        }
    }

    // ======================
    // NAVEGACIÓN DE MENÚ
    // ======================

    public void IrARegistro() => ActivarPanel(pRegistro);
    public void IrAConfig() => ActivarPanel(pConfigPartida);
    public void IrAOpciones() => ActivarPanel(pOpciones);
    public void IrACreditos() => ActivarPanel(pCreditos);
    public void IrATutorial() => ActivarPanel(pTutorial);
    public void VolverAlInicio() => ActivarPanel(pInicio);

    public void ConfirmarUsuario()
    {
        if (string.IsNullOrEmpty(inputNombre.text)) return;

        // Guardamos el nombre en la memoria
        PlayerPrefs.SetString("NombreUsuario", inputNombre.text);

        // ESTA ES LA LÍNEA QUE FALTA: Fuerza a Unity a escribir el archivo en el disco
        PlayerPrefs.Save();

        txtBienvenida.text = "Hola, " + inputNombre.text;
        ActivarPanel(pInicio);
    }

    // ======================
    // DIFICULTAD
    // ======================

    public void SeleccionarDificultad(int d)
    {
        PlayerPrefs.SetInt("Dificultad", d);
    }

    // ======================
    // SKIN (ROTACIÓN)
    // ======================
    float lastClick;
    public float delay = 0.2f;
    public void CambiarSkin()
    {
        if (Time.time - lastClick < delay) return;
        lastClick = Time.time;
        colorIndex = (colorIndex + 1) % coloresHex.Length;


        string hex = coloresHex[colorIndex];
        PlayerPrefs.SetString("SkinColor", hex);

        AplicarPreview(hex);
    }

    void CargarSkin()
    {
        if (PlayerPrefs.HasKey("SkinColor"))
        {
            string guardado = PlayerPrefs.GetString("SkinColor");
            colorIndex = System.Array.IndexOf(coloresHex, guardado);
            if (colorIndex < 0) colorIndex = 0;
        }
        else
        {
            PlayerPrefs.SetString("SkinColor", coloresHex[0]);
            colorIndex = 0;
        }

        AplicarPreview(coloresHex[colorIndex]);
    }

    void AplicarPreview(string hex)
    {
        Color c;
        if (ColorUtility.TryParseHtmlString(hex, out c))
            previewColor.color = c;

        if (textoSkin != null)
            textoSkin.text = $"Skin {colorIndex + 1}/{coloresHex.Length}";
    }

    // ======================
    // JUGAR
    // ======================

    public void Jugar()
    {
        int d = PlayerPrefs.GetInt("Dificultad", 0);
        string[] niveles = { "NivelFacil", "NivelMedio", "NivelDificil" };
        SceneManager.LoadScene(niveles[d]);
    }

    // ======================
    // OPCIONES
    // ======================

    public void AjustarMusica(float v)
    {
        PlayerPrefs.SetFloat("Vol", v);
        AudioListener.volume = v;
    }

    public void AjustarBrillo(float v)
    {
        PlayerPrefs.SetFloat("Brillo", v);
        Color c = filtroBrillo.color;
        c.a = 1f - v;
        filtroBrillo.color = c;
    }

    void CargarAjustes()
    {
        sliderMusica.value = PlayerPrefs.GetFloat("Vol", 0.7f);
        sliderBrillo.value = PlayerPrefs.GetFloat("Brillo", 1f);
        AjustarMusica(sliderMusica.value);
        AjustarBrillo(sliderBrillo.value);
    }

    // ======================
    // UTILIDADES
    // ======================

    void ActivarPanel(GameObject p)
    {
        pPortada.SetActive(false);
        pRegistro.SetActive(false);
        pInicio.SetActive(false);
        pConfigPartida.SetActive(false);
        pOpciones.SetActive(false);
        pCreditos.SetActive(false);
        pTutorial.SetActive(false);

        p.SetActive(true);
    }

    public void Salir()
    {
        Application.Quit();
    }

    [Header("Confirmación Salida")]
    public GameObject pConfirmarSalir; // Aquí arrastraremos el panel nuevo

    public void AbrirConfirmacion()
    {
        pConfirmarSalir.SetActive(true); // Muestra el cartel
    }

    public void CerrarConfirmacion()
    {
        pConfirmarSalir.SetActive(false); // Oculta el cartel
    }

    public void SalirDefinitivo()
    {
        Application.Quit(); // Cierra el juego
    }
}
