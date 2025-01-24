using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    [SerializeField] protected float rotationSpeed = 50f; // Rotation visuelle
    [SerializeField] protected AudioClip collectSound; // Son de collecte

    // Callback pour notifier la destruction de cet objet
    public System.Action OnDestroyCallback;

    private void Update()
    {
        // Animation de rotation
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        TankBehavior tank = other.GetComponent<TankBehavior>();
        if (tank != null)
        {
            OnCollected(tank); // Appelle la méthode abstraite
            PlayCollectSound();
            Destroy(gameObject); // Détruit le collectable après utilisation
        }
    }

    protected abstract void OnCollected(TankBehavior tank);

    private void PlayCollectSound()
    {
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }
    }

    private void OnDestroy()
    {
        OnDestroyCallback?.Invoke();
    }
}
