using UnityEngine;

public class Gema:MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameplayController.instance.RecogerGema();
            Destroy(gameObject);
        }
    }
}
