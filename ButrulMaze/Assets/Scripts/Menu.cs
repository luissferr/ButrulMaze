using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject pPortada, pRegistro, pInicio, pConfigPartida, pOpciones, pCreditos, pTutorial;
    public GameObject pConfirmarSalir;

    [Header("Usuario")]
    public InputField inputNombre;
    public Text txtBienvenida;
    public Text txtUsuarioLogueado;

    [Header("Opciones")]
    public Slider sliderMusica, sliderBrillo, sliderSFX;
    public Image filtroBrillo;

    [Header("Skin (color)")]
    public Renderer bolaPreview;
    public Text textoSkin;
    public string[] coloresHex = { "#FF0000", "#00FF00", "#0000FF", "#FFFF00", "#FF00FF", "#00FFFF" };

    int colorIndex = 0;

    [Header("Nivel Aleatorio")]
    public string[] escenasFaciles = { "NivelFacil", "NivelFacil2", "NivelFacil3" };
    public string[] escenasMedias = { "NivelMedio", "NivelMedio2", "NivelMedio3" };
    public string[] escenasDificiles = { "NivelDificil", "NivelDificil2", "NivelDificil3" };

    void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        ActualizarPerfilVisual();
        CargarAjustes();
        CargarSkin();

        
        string nombre = PlayerPrefs.GetString("NombreUsuario", "");
        if (!string.IsNullOrEmpty(nombre))
        {
            inputNombre.text = nombre;
            txtBienvenida.text = "Hola, " + nombre;
            ActivarPanel(pInicio);
        }
        else
        {
            ActivarPanel(pPortada);
        }
    }



    public void ConfirmarUsuario()
    {
        if (string.IsNullOrEmpty(inputNombre.text)) return;

        PlayerPrefs.SetString("NombreUsuario", inputNombre.text);
        PlayerPrefs.Save();

        
        ActualizarPerfilVisual();

        if (txtBienvenida != null) txtBienvenida.text = "Hola, " + inputNombre.text;

        ActivarPanel(pInicio);
    }

    // --- NAVEGACIÓN ---
    public void IrAInicio() => ActivarPanel(pInicio);
    public void IrAConfig() => ActivarPanel(pConfigPartida);
    public void IrAOpciones() => ActivarPanel(pOpciones);
    public void IrARegistro() => ActivarPanel(pRegistro);
    public void IrACreditos() => ActivarPanel(pCreditos);
    public void IrATutorial() => ActivarPanel(pTutorial);

    public void AbrirConfirmir() => pConfirmarSalir.SetActive(true);
    public void CerrarConf() => pConfirmarSalir.SetActive(false);
    public void SalirDefiniti() => Application.Quit();

    // --- SKINS ---
    public void CambiarSkin()
    {
        colorIndex = (colorIndex + 1) % coloresHex.Length;
        AplicarColor(coloresHex[colorIndex]);
        PlayerPrefs.SetString("SkinColor", coloresHex[colorIndex]);
        PlayerPrefs.Save();
        ActualizarTextoColor();
    }

    void AplicarColor(string hex)
    {
        Color c;
        if (ColorUtility.TryParseHtmlString(hex, out c))
        {
            if (bolaPreview != null)
                bolaPreview.material.color = c;
        }
    }


    void CargarSkin()
    {
        string colorGuardado = PlayerPrefs.GetString("SkinColor", coloresHex[0]);
        AplicarColor(colorGuardado);
        ActualizarTextoColor();
    }

    void ActualizarTextoColor() { if (textoSkin != null) textoSkin.text = "Color: " + (colorIndex + 1); }

    // --- AJUSTES  ---
    public void AjustarMusica(float v)
    {
        
        if (MusicManager.instance != null)
        {
            MusicManager.instance.CambiarVolumen(v);
        }
    }

    public void AjustarSFX(float v)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.CambiarVolumen(v);
        }
    }

    public void AjustarBrillo(float v)
    {
        if (BrightnessManager.instance != null)
            BrightnessManager.instance.AplicarBrillo(v);
    }


    void CargarAjustes()
    {
        float vol = PlayerPrefs.GetFloat("VolMusica", 0.7f);
        float bri = PlayerPrefs.GetFloat("Brillo", 1f);
        float sfx = PlayerPrefs.GetFloat("VolSFX", 0.7f);

        if (sliderMusica != null) sliderMusica.SetValueWithoutNotify(vol);
        if (sliderBrillo != null) sliderBrillo.SetValueWithoutNotify(bri);
        if (sliderSFX != null) sliderSFX.SetValueWithoutNotify(sfx);

        AjustarBrillo(bri);
    }


    // --- GESTOR DE PANELES ---
    void ActivarPanel(GameObject p)
    {
        if (pPortada) pPortada.SetActive(false);
        if (pRegistro) pRegistro.SetActive(false);
        if (pInicio) pInicio.SetActive(false);
        if (pConfigPartida) pConfigPartida.SetActive(false);
        if (pOpciones) pOpciones.SetActive(false);
        if (pCreditos) pCreditos.SetActive(false);
        if (pTutorial) pTutorial.SetActive(false);
        if (pConfirmarSalir) pConfirmarSalir.SetActive(false);

        if (p != null) p.SetActive(true);
    }

    public void JugarFacil()
    {
        CargarEscenaAlAzar(escenasFaciles);
    }

    public void JugarMedio()
    {
        CargarEscenaAlAzar(escenasMedias);
    }

    public void JugarDificil()
    {
        CargarEscenaAlAzar(escenasDificiles);
    }

    private void CargarEscenaAlAzar(string[] lista)
    {
        if (lista != null && lista.Length > 1)
        {
            
            int ultimoIndice = PlayerPrefs.GetInt("UltimoIndice_" + lista[0], -1);
            int nuevoIndice;

            do
            {
                nuevoIndice = Random.Range(0, lista.Length);
            } while (nuevoIndice == ultimoIndice);

           
            PlayerPrefs.SetInt("UltimoIndice_" + lista[0], nuevoIndice);
            SceneManager.LoadScene(lista[nuevoIndice]);
        }
        else if (lista != null && lista.Length == 1)
        {
            SceneManager.LoadScene(lista[0]);
        }
    }

    public void SeleccionarDificultad(int d) { PlayerPrefs.SetInt("Dificultad", d); }


    public void CambiarDeUsuario()
    {
        
        PlayerPrefs.SetString("NombreUsuario", "");
        PlayerPrefs.Save();

        if (inputNombre != null) inputNombre.text = "";

        ActualizarPerfilVisual();
        
        ActivarPanel(pRegistro);
    }

    public void ActualizarPerfilVisual()
    {
        if (txtUsuarioLogueado != null)
        {
            // Recuperamos el nombre guardado en PlayerPrefs
            string nombre = PlayerPrefs.GetString("NombreUsuario", "Invitado");
            txtUsuarioLogueado.text = nombre;
        }
    }
}