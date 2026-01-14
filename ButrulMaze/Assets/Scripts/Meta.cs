using UnityEngine;
using UnityEngine.SceneManagement;

public class Meta : MonoBehaviour
{
    private Renderer rend;

    [Header("Configuración")]
   
    public int idMaterialTela = 2;

    private Color colorBloqueado = Color.red;
    private Color colorDesbloqueado = Color.green;
    private bool yaDesbloqueada = false;

    void Start()
    {
        
        rend = GetComponentInChildren<Renderer>();

        if (rend == null)
        {
            Debug.LogError("¡ERROR! No encuentro el Mesh Renderer en la bandera.");
            return;
        }

        PintarBandera(colorBloqueado);
    }

    void Update()
    {
        if (yaDesbloqueada) return;

        if (GameplayController.instance.recolectadas >= GameplayController.instance.totales)
        {
            PintarBandera(colorDesbloqueado);
            yaDesbloqueada = true;
        }
    }

    void PintarBandera(Color nuevoColor)
    {
        if (rend == null) return;

        
        Material[] mats = rend.materials;

        if (idMaterialTela < mats.Length)
        {
            mats[idMaterialTela].color = nuevoColor;

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

        
        if (GameplayController.instance.recolectadas >= GameplayController.instance.totales)
        {
            
            if (SoundManager.instance != null)
                SoundManager.instance.Meta();

            
            GameplayController.instance.RegistrarVictoria();

            
            Invoke("IrAlRanking", 2f);
        }
    }

    void IrAlRanking()
    {
        SceneManager.LoadScene("Ranking");
    }

}