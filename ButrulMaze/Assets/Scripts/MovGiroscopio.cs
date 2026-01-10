using UnityEngine;

public class MovGiroscopio : MonoBehaviour
{
    public float sensibilidad = 1.0f;
    public float suavizado = 15.0f;
    public float maxAngulo = 10f; // Ángulo pequeño para que sea estable

    void FixedUpdate()
    {
        Vector3 inclinacion = Input.acceleration;

        // Calculamos la rotación
        float targetX = inclinacion.y * sensibilidad * 20f;
        float targetZ = -inclinacion.x * sensibilidad * 20f;

        // Limitamos para que no sea exagerado
        targetX = Mathf.Clamp(targetX, -maxAngulo, maxAngulo);
        targetZ = Mathf.Clamp(targetZ, -maxAngulo, maxAngulo);

        Quaternion targetRotation = Quaternion.Euler(targetX, 0, targetZ);

        // Aplicamos la rotación suave
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.fixedDeltaTime * suavizado);
    }
}