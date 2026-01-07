using UnityEngine;

public class Meta : MonoBehaviour
{
    private Renderer rend;

    public Color desbloqueada = Color.green;
    public Color bloqueada = Color.red;

    void Awake()
    {
        rend = GetComponent<Renderer>(); // Toma automáticamente el Renderer del objeto
    }

    void Update()
    {
        // Cambiar color según si todas las gemas están recogidas
        if (GameplayController.instance.recolectadas >= GameplayController.instance.totales)
            rend.material.color = desbloqueada;
        else
            rend.material.color = bloqueada;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Solo permitir ganar si todas las gemas fueron recogidas
        if (GameplayController.instance.recolectadas >= GameplayController.instance.totales)
        {
            GameplayController.instance.IntentarGanar();
        }
        else
        {
            Debug.Log("Recoge todas las gemas antes de entrar a la meta!");
        }
    }
}
