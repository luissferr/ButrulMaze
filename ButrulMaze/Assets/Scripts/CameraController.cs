using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform objetivo;
    public Vector3 offset = new Vector3(0, 18, -4); 
    public float suavizado = 5f;

    void LateUpdate()
    {
        if (objetivo == null) return;

        Vector3 posicionDeseada = objetivo.position + offset;

        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);

        transform.rotation = Quaternion.Euler(75, 0, 0);
    }
}