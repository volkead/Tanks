using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [Header("Zone de Spawn")]
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(10, 1, 10); // Taille de la zone
    [SerializeField] private Vector3 spawnAreaOffset = Vector3.zero;         // D�calage de la zone

    [Header("Collectables")]
    [SerializeField] private List<GameObject> collectables; // Liste des prefabs collectables
    [SerializeField] private int maxCollectables = 10;      // Nombre maximum d'objets en m�me temps
    [SerializeField] private float spawnInterval = 5f;      // Temps entre chaque tentative de spawn

    private List<GameObject> spawnedCollectables = new List<GameObject>(); // Liste des objets actifs dans la sc�ne

    private void Start()
    {
        // Lance la coroutine qui g�re le spawn en boucle
        StartCoroutine(SpawnCollectablesRoutine());
    }

    private IEnumerator SpawnCollectablesRoutine()
    {
        while (true)
        {
            if (spawnedCollectables.Count < maxCollectables)
            {
                SpawnCollectable();
            }
            yield return new WaitForSeconds(spawnInterval); // Attente avant de v�rifier � nouveau
        }
    }

    private void SpawnCollectable()
    {
        if (collectables.Count == 0)
        {
            Debug.LogWarning("Aucun prefab collectable n'a �t� d�fini !");
            return;
        }

        // S�lectionne un prefab al�atoire
        GameObject prefabToSpawn = collectables[Random.Range(0, collectables.Count)];

        // G�n�re une position al�atoire dans la zone de spawn
        Vector3 randomPosition = GetRandomPositionInSpawnArea();

        // Instancie l'objet collectable
        GameObject spawnedObject = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);
        spawnedCollectables.Add(spawnedObject); // Ajoute � la liste des collectables actifs

        // Nettoie la liste des collectables lorsqu'ils sont d�truits
        Collectable collectableComponent = spawnedObject.GetComponent<Collectable>();

        if (collectableComponent != null)
        {
            collectableComponent.OnDestroyCallback += () =>
            {
                spawnedCollectables.Remove(spawnedObject);
            };
        }
    }

    private Vector3 GetRandomPositionInSpawnArea()
    {
        // G�n�re une position al�atoire dans les limites de la zone de spawn
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        // Retourne la position avec le d�calage pris en compte
        return transform.position + spawnAreaOffset + randomPosition;
    }

    private void OnDrawGizmos()
    {
        // Dessine la zone de spawn dans l'�diteur
        Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.4f);
        Gizmos.DrawCube(transform.position + spawnAreaOffset, spawnAreaSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + spawnAreaOffset, spawnAreaSize);
    }
}
