using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Range(2, 4)]
    public int nbPlayer; // Nombre de joueurs
    [SerializeField] GameObject tankPrefab; // Prefab de tank

    private List<EPlayer> activePlayerIDs = new List<EPlayer>(); // Liste des IDs d�j� attribu�s

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void InstantiatePlayers()
    {
        for (int i = 0; i < nbPlayer; i++)
        {
            // V�rifie si un playerID est d�j� actif
            EPlayer playerID = (EPlayer)i;
            if (activePlayerIDs.Contains(playerID))
            {
                Debug.LogWarning($"Un tank avec le PlayerID {playerID} existe d�j�. Ignor�.");
                continue;
            }

            // Instancie un nouveau tank et configure son playerID
            GameObject tank = Instantiate(tankPrefab);
            TankBehavior tankBehavior = tank.GetComponent<TankBehavior>();

            if (tankBehavior != null)
            {
                tankBehavior.playerID = playerID;
                activePlayerIDs.Add(playerID); // Ajoute le playerID � la liste des IDs actifs
            }
            else
            {
                Debug.LogError("Le prefab du tank n'a pas de script TankBehavior !");
            }

            // Emp�che la destruction du tank lors du changement de sc�ne
            DontDestroyOnLoad(tank);
        }
    }

    public void ResetActivePlayers()
    {
        activePlayerIDs.Clear(); // R�initialise la liste des IDs actifs
    }
}
