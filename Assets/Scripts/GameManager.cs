using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int currentPlayerCount = 0; // Compte les joueurs connect�s

    private void Awake()
    {
        var playerInputManager = PlayerInputManager.instance;
        if (playerInputManager == null)
        {
            Debug.LogError("PlayerInputManager instance is null. Ensure it exists in the scene.");
            return;
        }

        playerInputManager.onPlayerJoined += OnPlayerJoined;
    }


    private void OnPlayerJoined(PlayerInput playerInput)
    {
        // Assigne un PlayerID bas� sur l'ordre d'arriv�e
        var tankBehavior = playerInput.GetComponent<TankBehavior>();
        if (tankBehavior != null)
        {
            //tankBehavior.playerID = (EPlayer)currentPlayerCount;
            currentPlayerCount++;

            Debug.Log($"Player {tankBehavior.playerID} joined with device: {playerInput.devices[0].name}");
        }

        // Si le nombre max de joueurs est atteint, d�sactiver le join automatique
        if (currentPlayerCount >= PlayerInputManager.instance.maxPlayerCount)
        {
            PlayerInputManager.instance.DisableJoining();
        }
    }
}
