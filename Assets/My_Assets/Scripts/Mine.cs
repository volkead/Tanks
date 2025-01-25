using System.Collections;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private float explosionDelay = 10f; // Délai avant explosion si pas de contact
    [SerializeField] private int damage = 10; // Dégâts infligés à un tank
    [SerializeField] private float explosionRadius = 5f; // Rayon d'explosion
    [SerializeField] private ParticleSystem explosionEffect; // VFX de l'explosion
    [SerializeField] private AudioClip explosionSound; // SFX de l'explosion

    private bool hasExploded = false;

    private void Start()
    {
        // Commence le compte à rebours pour l'explosion si pas de contact
        StartCoroutine(ExplosionCountdown());
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si la mine touche un tank, elle explose et inflige des dégâts
        if (other.CompareTag("Character") && !hasExploded)
        {
            Explode(other);
        }
    }

    private IEnumerator ExplosionCountdown()
    {
        // Attendre avant l'explosion
        yield return new WaitForSeconds(explosionDelay);

        // Si toujours pas explosée, elle explose
        if (!hasExploded)
        {
            Explode(null);
        }
    }

    private void Explode(Collider tank)
    {
        if (hasExploded) return;
        hasExploded = true;

        // Effets visuels et sonores
        if (explosionEffect) Instantiate(explosionEffect, transform.position, Quaternion.identity);
        if (explosionSound) AudioSource.PlayClipAtPoint(explosionSound, transform.position);

        // Inflige des dégâts aux tanks dans le rayon
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Character"))
            {
                TankBehavior tankBehavior = collider.GetComponent<TankBehavior>();
                if (tankBehavior != null) tankBehavior.Hit(damage);
            }
        }

        // Détruire la mine après l'explosion
        Destroy(gameObject);
    }

    // Affiche un Gizmo dans l'éditeur pour visualiser le rayon d'explosion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius); 
    }
}
