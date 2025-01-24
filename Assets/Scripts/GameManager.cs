using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<PlayerControls> playerControls; // Liste des contr�les associ�s � chaque joueur
    [SerializeField] List<TankBehavior> tanks; // Liste des tanks

    // D�clare l'�v�nement Win
    public delegate void WinEvent(EPlayer winner);
    public event WinEvent OnWin;

    private void Awake()
    {
        // Trouve tous les tanks dans la sc�ne et les ajoute � la liste
        tanks.AddRange(FindObjectsOfType<TankBehavior>());

        // Abonne les tanks � l'�v�nement de mort
        foreach (var tank in tanks)
        {
            tank.OnTankDeath += onTankKilled;
        }
    }

    private void OnDestroy()
    {
        // D�sabonne les tanks de l'�v�nement de mort lorsque le contr�leur est d�truit
        foreach (var tank in tanks)
        {
            tank.OnTankDeath -= onTankKilled;
        }
    }

    void Update()
    {
        for (int i = 0; i < tanks.Count; i++)
        {
            // Assurez-vous que le tank n'est pas mort avant d'essayer de l'utiliser
            if (tanks[i] != null)
            {
                // R�cup�re les contr�les pour ce joueur
                PlayerControls controls = playerControls[i];

                // D�placement avant/arri�re
                if (Input.GetKey(controls.moveForward))
                {
                    tanks[i].Move(Vector3.forward);
                }
                if (Input.GetKey(controls.moveBackward))
                {
                    tanks[i].Move(Vector3.back);
                }

                // Rotation gauche/droite
                if (Input.GetKey(controls.turnRight))
                {
                    tanks[i].Turn(1);
                }
                if (Input.GetKey(controls.turnLeft))
                {
                    tanks[i].Turn(-1);
                }

                // Gestion du tir avec chargement
                if (Input.GetKeyDown(controls.shoot))
                {
                    tanks[i].StartCharge(); // Commencer � charger
                }
                if (Input.GetKey(controls.shoot))
                {
                    tanks[i].Charge(); // Maintenir la charge
                }
                if (Input.GetKeyUp(controls.shoot))
                {
                    tanks[i].ShootWithPower(); // Tirer avec la puissance accumul�e
                }

                // Poser une bombe
                if (Input.GetKeyDown(controls.dropBomb))
                {
                    tanks[i].DropBomb();
                }
            }
        }
    }

    // M�thode appel�e lorsque le tank meurt
    private void onTankKilled(EPlayer playerID)
    {
        Debug.Log($"Le tank du joueur {playerID} est mort.");

        // Retirer le tank mort de la liste
        tanks.RemoveAll(tank => tank == null || tank.playerID == playerID);

        // V�rifie s'il reste un seul tank dans la liste
        List<TankBehavior> remainingTanks = tanks.FindAll(tank => tank != null);
        if (remainingTanks.Count == 1)
        {
            // D�clenche l'�v�nement Win si un seul tank reste
            EPlayer winner = remainingTanks[0].playerID;
            OnWin?.Invoke(winner);
            Debug.Log($"Le joueur {winner} a gagn� !");
        }
    }
}

[System.Serializable]
public class PlayerControls
{
    public string name = "Player";
    public KeyCode moveForward;
    public KeyCode moveBackward;
    public KeyCode turnLeft;
    public KeyCode turnRight;
    public KeyCode shoot;
    public KeyCode dropBomb;
}
