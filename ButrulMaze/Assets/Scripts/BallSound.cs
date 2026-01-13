using UnityEngine;

public class BallSound:MonoBehaviour
{
    float cooldown = 0.1f;
    float ultimoSonido = 0f;

    void OnCollisionEnter(Collision col)
    {
        if (!col.gameObject.CompareTag("Pared")) return;

        // Medimos la fuerza del impacto
        float fuerza = col.relativeVelocity.magnitude;

        // Solo suena si el golpe es suficientemente fuerte
        if (fuerza > 1.5f && Time.time - ultimoSonido > cooldown)
        {
            if (SoundManager.instance != null)
                SoundManager.instance.Choque();

            ultimoSonido = Time.time;
        }
    }
}
