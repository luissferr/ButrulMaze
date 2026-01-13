using UnityEngine;

public class MovGiroscopio : MonoBehaviour
{
    public float sensibilidad = 1.0f;
    public float suavizado = 15.0f;
    public float maxAngulo = 10f;

    void FixedUpdate()
    {
        // Obtenemos la aceleración del dispositivo
        Vector3 inclinacion = Input.acceleration;

        // --- AJUSTE PARA MODO HORIZONTAL (LANDSCAPE) ---

        // 1. targetX (Rotación adelante/atrás del tablero):
        // En horizontal, esto corresponde al eje X físico del móvil.
        // Si sientes que va al revés, quita o por el signo negativo en 'inclinacion.x'.
        float targetX = inclinacion.x * sensibilidad * 20f;

        // 2. targetZ (Rotación izquierda/derecha del tablero):
        // En horizontal, esto corresponde al eje Y físico del móvil.
        // Si sientes que va al revés, quita o pon el signo negativo en 'inclinacion.y'.
        float targetZ = inclinacion.y * sensibilidad * 20f;

        // Limitamos para que no sea exagerado (Clamp)
        targetX = Mathf.Clamp(targetX, -maxAngulo, maxAngulo);
        targetZ = Mathf.Clamp(targetZ, -maxAngulo, maxAngulo);

        // Creamos la rotación objetivo
        Quaternion targetRotation = Quaternion.Euler(targetX, 0, targetZ);

        // Aplicamos la rotación suavemente
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.fixedDeltaTime * suavizado);
    }

}