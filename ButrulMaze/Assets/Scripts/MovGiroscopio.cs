using UnityEngine;

public class MovGiroscopio : MonoBehaviour
{
    public float sensibilidad = 2.0f;
    public float suavizado = 5.0f;
    public float maxAngulo = 15f;

    void Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
    }

    void FixedUpdate()
    {
        
        Vector3 acc = Input.acceleration;

        if (acc == Vector3.zero && SystemInfo.supportsGyroscope)
        {
            acc = Input.gyro.gravity;
        }

      
        float targetX = acc.y * sensibilidad * 20f;
      
        float targetZ = -acc.x * sensibilidad * 20f;

        targetX = Mathf.Clamp(targetX, -maxAngulo, maxAngulo);
        targetZ = Mathf.Clamp(targetZ, -maxAngulo, maxAngulo);

        Quaternion targetRotation = Quaternion.Euler(targetX, 0, targetZ);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.fixedDeltaTime * suavizado);
    }
}