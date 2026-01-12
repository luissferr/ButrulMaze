using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject pPortada;
    public GameObject pRegistro, pInicio, pConfigPartida, pOpciones, pCreditos, pTutorial, pConfirmarSalir;

    [Header("Usuario")]
    public InputField inputNombre;
    public Text txtBienvenida;

    [Header("Opciones")]
    public Slider sliderMusica, sliderBrillo;
    public Image filtroBrillo;

    [Header("Skin (color)")]
    public Renderer bolaPreview; // <--- ARRASTRA AQUÍ LA BOLA 3D DE LA ESCENA
    public Text textoSkin;
    public string[] coloresHex = { "#FF0000", "#00FF00", "#0000FF", "#FFFF00", "#FF00FF", "#00FFFF" };
    int colorIndex = 0;

    void Start()
    {
        CargarAjustes();
        CargarSkin();

        if (PlayerPrefs.HasKey("NombreUsuario"))
        {
            inputNombre.text = PlayerPrefs.GetString("NombreUsuario");
            txtBienvenida.text = "Hola, " + PlayerPrefs.GetString("NombreUsuario");
            ActivarPanel(pInicio);
        }
        else
        {
            ActivarPanel(pPortada);
        }
    }

    public void ConfirmarUsuario()
    {
        if (inputNombre == null || string.IsNullOrEmpty(inputNombre.text)) return;

        PlayerPrefs.SetString("NombreUsuario", inputNombre.text);
        txtBienvenida.text = "Hola, " + inputNombre.text;
        ActivarPanel(pInicio);
    }

    public void CambiarSkin()
    {
        colorIndex = (colorIndex + 1) % coloresHex.Length;
        string hex = coloresHex[colorIndex];
        PlayerPrefs.SetString("SkinColor", hex);
        AplicarPreview(hex);
    }

    void CargarSkin()
    {
        string guardado = PlayerPrefs.GetString("SkinColor", coloresHex[0]);
        colorIndex = System.Array.IndexOf(coloresHex, guardado);
        if (colorIndex < 0) colorIndex = 0;
        AplicarPreview(coloresHex[colorIndex]);
    }

    void AplicarPreview(string hex)
    {
        Color c;
        if (ColorUtility.TryParseHtmlString(hex, out c))
        {
            if (bolaPreview != null) bolaPreview.material.color = c; // <--- CAMBIO CLAVE
        }

        if (textoSkin != null)
            textoSkin.text = $"Skin {colorIndex + 1}/{coloresHex.Length}";
    }

    public void SeleccionarDificultad(int d) => PlayerPrefs.SetInt("Dificultad", d);

    public void Jugar()
    {
        int d = PlayerPrefs.GetInt("Dificultad", 0);
        string[] niveles = { "NivelFacil", "NivelMedio", "NivelDificil" };
        SceneManager.LoadScene(niveles[d]);
    }

    // --- NAVEGACIÓN ---
    public void IrARegistro() => ActivarPanel(pRegistro);
    public void IrAConfig() => ActivarPanel(pConfigPartida);
    public void IrAOpciones() => ActivarPanel(pOpciones);
    public void IrACreditos() => ActivarPanel(pCreditos);
    public void IrATutorial() => ActivarPanel(pTutorial);
    public void VolverAlInicio() => ActivarPanel(pInicio);

    void ActivarPanel(GameObject p)
    {
        pPortada.SetActive(false); pRegistro.SetActive(false); pInicio.SetActive(false);
        pConfigPartida.SetActive(false); pOpciones.SetActive(false); pCreditos.SetActive(false);
        pTutorial.SetActive(false);
        if (pConfirmarSalir != null) pConfirmarSalir.SetActive(false);
        p.SetActive(true);
    }

    // --- OPCIONES ---
    public void AjustarMusica(float v) { PlayerPrefs.SetFloat("Vol", v); AudioListener.volume = v; }
    public void AjustarBrillo(float v)
    {
        PlayerPrefs.SetFloat("Brillo", v);
        Color c = filtroBrillo.color; c.a = 1f - v; filtroBrillo.color = c;
    }
    void CargarAjustes()
    {
        sliderMusica.value = PlayerPrefs.GetFloat("Vol", 0.7f);
        sliderBrillo.value = PlayerPrefs.GetFloat("Brillo", 1f);
        AjustarMusica(sliderMusica.value); AjustarBrillo(sliderBrillo.value);
    }
    public void Salir() => Application.Quit();
}