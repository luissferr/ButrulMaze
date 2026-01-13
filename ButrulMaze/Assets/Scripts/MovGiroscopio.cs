using UnityEngine;

public class MovGiroscopio : MonoBehaviour
{
    public float sensibilidad = 2.0f;
    public float suavizado = 5.0f;
    public float maxAngulo = 15f;

    void Start()
    {
        // ESTA ES LA CLAVE: Algunos móviles necesitan que se active el giroscopio
        // aunque estemos usando Input.acceleration
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
    }

    void FixedUpdate()
    {
        // Usamos acelerómetro (que es lo que mejor funciona en Landscape)
        Vector3 acc = Input.acceleration;

        // Si el sensor no envía nada, intentamos leer el gyro como respaldo
        if (acc == Vector3.zero && SystemInfo.supportsGyroscope)
        {
            acc = Input.gyro.gravity;
        }

        // --- EJES PARA MODO HORIZONTAL (LANDSCAPE) ---
        // Movimiento adelante/atrás
        float targetX = acc.y * sensibilidad * 20f;
        // Movimiento izquierda/derecha
        float targetZ = -acc.x * sensibilidad * 20f;

        targetX = Mathf.Clamp(targetX, -maxAngulo, maxAngulo);
        targetZ = Mathf.Clamp(targetZ, -maxAngulo, maxAngulo);

        Quaternion targetRotation = Quaternion.Euler(targetX, 0, targetZ);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.fixedDeltaTime * suavizado);
    }
}