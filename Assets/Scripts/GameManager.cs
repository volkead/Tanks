using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int currentPlayerCount = 0; // Compte les joueurs connectés

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
        // Assigne un PlayerID basé sur l'ordre d'arrivée
        var tankBehavior = playerInput.GetComponent<TankBehavior>();
        if (tankBehavior != null)
        {
            //tankBehavior.playerID = (EPlayer)currentPlayerCount;
            currentPlayerCount++;

            Debug.Log($"Player {tankBehavior.playerID} joined with device: {playerInput.devices[0].name}");
        }

        // Si le nombre max de joueurs est atteint, désactiver le join automatique
        if (currentPlayerCount >= PlayerInputManager.instance.maxPlayerCount)
        {
            PlayerInputManager.instance.DisableJoining();
        }
    }
}
