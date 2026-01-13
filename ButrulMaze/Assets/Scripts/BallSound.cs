using UnityEngine;

public class BallSound : MonoBehaviour
{
    private float ultimoSonido = 0f;
    private GameObject ultimaPared = null;

    [Header("Configuración")]
    public float cooldown = 0.15f;
    public float fuerzaMinima = 0.2f; // El valor que te iba bien al principio

    void OnCollisionEnter(Collision col)
    {
        if (!col.gameObject.CompareTag("Pared")) return;

        float fuerza = col.relativeVelocity.magnitude;

        // Si es una pared nueva o ha pasado el tiempo, suena
        if (fuerza > fuerzaMinima)
        {
            if (col.gameObject != ultimaPared || Time.time > ultimoSonido + cooldown)
            {
                SoundManager.instance?.Choque();
                ultimoSonido = Time.time;
                ultimaPared = col.gameObject;
            }
        }
    }

    // ESTO ES LA CLAVE PARA LOS PASILLOS:
    // Cuando la bola deja de tocar una pared (aunque siga tocando otra), 
    // "reseteamos" para que el siguiente choque entre limpio.
    void OnCollisionExit(Collision col)
    {
        if (col.gameObject == ultimaPared)
        {
            ultimaPared = null;
        }
    }
}