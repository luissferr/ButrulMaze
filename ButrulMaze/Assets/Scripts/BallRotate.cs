using UnityEngine;

public class BallRotate:MonoBehaviour
{
    void Update()
    {
        transform.Rotate(15f * Time.deltaTime, 90f * Time.deltaTime, 0); 
    }

}
