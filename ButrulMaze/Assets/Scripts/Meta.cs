using UnityEngine;

public class Meta : MonoBehaviour
{
    private Renderer rend;

    [Header("Configuración")]
    // Aquí definimos que queremos pintar el TERCER material de la lista (el 0, el 1 y el 2)
    public int idMaterialTela = 2;

    private Color colorBloqueado = Color.red;
    private Color colorDesbloqueado = Color.green;
    private bool yaDesbloqueada = false;

    void Start()
    {
        // Buscamos el Renderer (en este objeto o en los hijos)
        rend = GetComponentInChildren<Renderer>();

        if (rend == null)
        {
            Debug.LogError("¡ERROR! No encuentro el Mesh Renderer en la bandera.");
            return;
        }

        // Al empezar, pintamos la bandera de ROJO (bloqueada)
        PintarBandera(colorBloqueado);
    }

    void Update()
    {
        if (yaDesbloqueada) return;

        // Si ya tenemos todas las gemas...
        if (GameplayController.instance.recolectadas >= GameplayController.instance.totales)
        {
            // ...la pintamos de VERDE
            PintarBandera(colorDesbloqueado);
            yaDesbloqueada = true;
        }
    }

    void PintarBandera(Color nuevoColor)
    {
        if (rend == null) return;

        // Obtenemos la lista de materiales del modelo
        Material[] mats = rend.materials;

        // Verificamos que el Element 2 existe para no dar error
        if (idMaterialTela < mats.Length)
        {
            // Solo cambiamos el color del Element 2 (la tela)
            mats[idMaterialTela].color = nuevoColor;

            // Aplicamos el cambio al objeto
            rend.materials = mats;
        }
        else
        {
            Debug.LogWarning("¡Cuidado! El modelo no tiene Element " + idMaterialTela);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // SOLO si ya está desbloqueada (todas las gemas recogidas)
        if (GameplayController.instance.recolectadas >= GameplayController.instance.totales)
        {
            // Sonido de victoria
            if (SoundManager.instance != null)
                SoundManager.instance.Meta();

            // Ganar partida
            GameplayController.instance.IntentarGanar();
        }
        else
        {
            Debug.Log("¡Faltan gemas! La bandera sigue roja.");
        }
    }

}