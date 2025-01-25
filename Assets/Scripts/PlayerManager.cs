using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerInput> players = new List<PlayerInput>();

    [SerializeField]
    private List<Transform> startingPoints;

    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        playerInputManager = FindObjectOfType<PlayerInputManager>();

        if (playerInputManager == null)
        {
            Debug.LogError("PlayerInputManager not found in the scene!");
        }
    }

    private void OnEnable()
    {
        // Subscribe to the PlayerInputManager's event
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        // Unsubscribe from the PlayerInputManager's event
        playerInputManager.onPlayerJoined -= AddPlayer;
    }

    private void AddPlayer(PlayerInput player)
    {
        if (player.devices.Count == 0)
        {
            Debug.LogError("Player does not have any devices assigned!");
            return;
        }

        players.Add(player);

        InputDevice assignedDevice = player.devices[0];
        Debug.Log($"Player {players.Count} assigned device: {assignedDevice.displayName}");

        foreach (var device in player.devices)
        {
            Debug.Log($"Device assigned: {device.displayName}");
        }

        player.SwitchCurrentControlScheme(player.currentControlScheme, assignedDevice);
    }

}
