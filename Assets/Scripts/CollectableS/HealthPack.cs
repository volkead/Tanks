using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : Collectable
{
    [SerializeField] private int healthValue = 25; // Valeur de soin

    protected override void OnCollected(TankBehavior tank)
    {
        tank.Heal(healthValue); // Ajoute de la vie au tank
        Debug.Log("Health pack collected: +" + healthValue + " HP");
    }
}
