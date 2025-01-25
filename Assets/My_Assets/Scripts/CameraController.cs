using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float smoothTime = 0.2f; // Temps pour lisser les mouvements
    [SerializeField] private float minDistance = 10f;  // Distance minimale
    [SerializeField] private float maxDistance = 25f;  // Distance maximale
    [SerializeField] private float padding = 2f;       // Marge autour des joueurs
    [SerializeField] private float cameraHeight = 15f; // Hauteur de la caméra
    [SerializeField] private float cameraAngle = 45f;  // Angle de la caméra (en degrés)
    [SerializeField] private float minCameraHeight = 10f; // Hauteur minimale de la caméra

    private List<Transform> playerTransforms = new List<Transform>();  // Positions des joueurs
    private Vector3 velocity;              // Pour le lissage
    private Vector3 previousPosition;      // Pour calculer la vitesse de la caméra

    private void Start()
    {
        // Trouve tous les joueurs dans la scène
        TankBehavior[] tanks = FindObjectsOfType<TankBehavior>();
        foreach (var tank in tanks)
        {
            playerTransforms.Add(tank.transform);
        }

        // Configure l'angle initial de la caméra
        transform.rotation = Quaternion.Euler(cameraAngle, 0, 0);
        previousPosition = transform.position;  // Initialiser la position précédente
    }

    private void LateUpdate()
    {
        // Nettoie les références invalides (les tanks morts)
        if (playerTransforms.Count == 0) return;
        playerTransforms.RemoveAll(t => t == null);

        // Calcul de la zone englobante
        Bounds bounds = CalculateBounds();

        // Calcul de la distance optimale de la caméra
        float distance = Mathf.Clamp(bounds.extents.magnitude + padding, minDistance, maxDistance);

        // Calcul de la position cible de la caméra
        Vector3 targetPosition = bounds.center + Vector3.up * Mathf.Max(cameraHeight, minCameraHeight) - transform.forward * distance;

        // Lissage de la position de la caméra
        // Utilisation de Lerp pour un mouvement plus fluide
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / smoothTime);

        // Ajuster le plan de clipping de la caméra
        mainCamera.nearClipPlane = Mathf.Max(distance / 10f, 0.3f);
    }

    private Bounds CalculateBounds()
    {
        if (playerTransforms.Count == 1)
        {
            return new Bounds(playerTransforms[0].position, Vector3.zero);
        }

        // Initialise le `Bounds` avec le premier joueur
        Bounds bounds = new Bounds(playerTransforms[0].position, Vector3.zero);
        for (int i = 1; i < playerTransforms.Count; i++)
        {
            // Si le joueur est toujours présent, on l'ajoute à la zone englobante
            if (playerTransforms[i] != null)
            {
                bounds.Encapsulate(playerTransforms[i].position);
            }
        }

        return bounds;
    }
}
