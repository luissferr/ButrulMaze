using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Objetivo a Seguir")]
    public Transform objetivo; 

    [Header("Configuración")]
    public Vector3 offset; 
    public float suavizado = 0.125f; 

    void Start()
    {
        
        if (objetivo != null)
        {
            offset = transform.position - objetivo.position;
        }
    }

    void LateUpdate()
    {
       

        if (objetivo == null) return;

       
        Vector3 posicionDeseada = objetivo.position + offset;

        
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, suavizado);

        transform.position = posicionSuavizada;

        // Opcional: Descomenta la siguiente línea si quieres que la cámara siempre mire a la bola
        // transform.LookAt(objetivo);
    }
}