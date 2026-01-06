using UnityEngine;


public class MovGiroscopio : MonoBehaviour
{
    private Gyroscope giro;
    private bool giroSoportado;

    [Header("Configuración")]
    public float sensibilidad = 2.5f;
    public float suavizado = 6.0f;
    public float maxAngulo = 22f; // Limita la inclinación para que sea jugable

    void Start()
    {
        giroSoportado = SystemInfo.supportsGyroscope;
        if (giroSoportado)
        {
            giro = Input.gyro;
            giro.enabled = true;
        }
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Update()
    {
        if (giroSoportado)
        {
            // Rotación basada en la velocidad angular del giroscopio
            Vector3 rotGiro = giro.rotationRateUnbiased;
            float tX = rotGiro.x * sensibilidad;
            float tZ = -rotGiro.y * sensibilidad;

            Quaternion targetRot = Quaternion.Euler(
                Mathf.Clamp(NormalizeAngle(transform.localEulerAngles.x + tX), -maxAngulo, maxAngulo),
                0,
                Mathf.Clamp(NormalizeAngle(transform.localEulerAngles.z + tZ), -maxAngulo, maxAngulo)
            );

            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * suavizado);
        }
        else
        {
            // Acelerómetro como respaldo (Fallback)
            Vector3 acc = Input.acceleration;
            Quaternion targetAcc = Quaternion.Euler(acc.y * maxAngulo, 0, -acc.x * maxAngulo);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetAcc, Time.deltaTime * suavizado);
        }
    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180) angle -= 360;
        return angle;
    }
}
