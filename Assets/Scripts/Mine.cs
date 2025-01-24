using System.Collections;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private float explosionDelay = 10f; // D�lai avant explosion si pas de contact
    [SerializeField] private int damage = 10; // D�g�ts inflig�s � un tank
    [SerializeField] private float explosionRadius = 5f; // Rayon d'explosion
    [SerializeField] private ParticleSystem explosionEffect; // VFX de l'explosion
    [SerializeField] private AudioClip explosionSound; // SFX de l'explosion

    private bool hasExploded = false;

    private void Start()
    {
        // Commence le compte � rebours pour l'explosion si pas de contact
        StartCoroutine(ExplosionCountdown());
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si la mine touche un tank, elle explose et inflige des d�g�ts
        if (other.CompareTag("Character") && !hasExploded)
        {
            Explode(other);
        }
    }

    private IEnumerator ExplosionCountdown()
    {
        // Attendre avant l'explosion
        yield return new WaitForSeconds(explosionDelay);

        // Si toujours pas explos�e, elle explose
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

        // Inflige des d�g�ts aux tanks dans le rayon
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Character"))
            {
                TankBehavior tankBehavior = collider.GetComponent<TankBehavior>();
                if (tankBehavior != null) tankBehavior.Hit(damage);
            }
        }

        // D�truire la mine apr�s l'explosion
        Destroy(gameObject);
    }

    // Affiche un Gizmo dans l'�diteur pour visualiser le rayon d'explosion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius); 
    }
}
