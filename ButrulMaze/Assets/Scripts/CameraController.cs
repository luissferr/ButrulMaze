using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform objetivo;
    public Vector3 offset = new Vector3(0, 18, -4); // Valores por defecto estables
    public float suavizado = 5f;

    void LateUpdate()
    {
        if (objetivo == null) return;

        // 1. Calculamos la posición siguiendo a la bola
        Vector3 posicionDeseada = objetivo.position + offset;

        // 2. Movimiento suave independiente de los frames
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);

        // 3. Forzamos la rotación para que el tablero al inclinarse no mueva la cámara
        // Esto soluciona que los muros tapen la bola
        transform.rotation = Quaternion.Euler(75, 0, 0);
    }
}