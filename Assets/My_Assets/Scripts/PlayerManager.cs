using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Range(2, 4)]
    public int nbPlayer; // Nombre de joueurs
    [SerializeField] GameObject tankPrefab; // Prefab de tank

    private List<EPlayer> activePlayerIDs = new List<EPlayer>(); // Liste des IDs déjà attribués

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void InstantiatePlayers()
    {
        for (int i = 0; i < nbPlayer; i++)
        {
            // Vérifie si un playerID est déjà actif
            EPlayer playerID = (EPlayer)i;
            if (activePlayerIDs.Contains(playerID))
            {
                Debug.LogWarning($"Un tank avec le PlayerID {playerID} existe déjà. Ignoré.");
                continue;
            }

            // Instancie un nouveau tank et configure son playerID
            GameObject tank = Instantiate(tankPrefab);
            TankBehavior tankBehavior = tank.GetComponent<TankBehavior>();

            if (tankBehavior != null)
            {
                tankBehavior.playerID = playerID;
                activePlayerIDs.Add(playerID); // Ajoute le playerID à la liste des IDs actifs
            }
            else
            {
                Debug.LogError("Le prefab du tank n'a pas de script TankBehavior !");
            }

            // Empêche la destruction du tank lors du changement de scène
            DontDestroyOnLoad(tank);
        }
    }

    public void ResetActivePlayers()
    {
        activePlayerIDs.Clear(); // Réinitialise la liste des IDs actifs
    }
}
