using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : Collectable
{
    [SerializeField] private float duration = 10f; // Durée du power-up
    [SerializeField] private PowerUpType powerUpType; // Type du power-up

    protected override void OnCollected(TankBehavior tank)
    {
        tank.ActivatePowerUp(powerUpType, duration); // Active le power-up sur le tank
        Debug.Log($"Power-up collected: {powerUpType} for {duration} seconds");
    }
}

public enum PowerUpType
{
    SpeedBoost,
    Invincibility,
    DoubleDamage
}
