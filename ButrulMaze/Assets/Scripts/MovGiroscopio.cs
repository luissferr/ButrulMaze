using UnityEngine;

public class MovGiroscopio : MonoBehaviour
{
    [Header("Configuración de Control")]
    public float sensibilidad = 1.5f;
    public float suavizado = 10.0f;
    public float maxAngulo = 18f; 

    void Start()
    {
        
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }

       
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

       
        transform.localRotation = Quaternion.identity;
    }

    void Update()
    {
       
        Vector3 inclinacion = Input.acceleration;

        
        float targetX = -inclinacion.y * maxAngulo * sensibilidad;
        float targetZ = inclinacion.x * maxAngulo * sensibilidad;

        
        targetX = Mathf.Clamp(targetX, -maxAngulo, maxAngulo);
        targetZ = Mathf.Clamp(targetZ, -maxAngulo, maxAngulo);

        
        Quaternion targetRotation = Quaternion.Euler(targetX, 0, targetZ);

 
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * suavizado);

       
        transform.localPosition = Vector3.zero;
    }
}