using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : Collectable
{
    [SerializeField] private int ammoValue = 5; // Nombre de munitions donn�es
    [SerializeField] private int mineValue = 2; // Nombre de mines donn�es

    protected override void OnCollected(TankBehavior tank)
    {
        tank.ammo += ammoValue; // Ajoute des munitions
        tank.mineAmmo += mineValue; // Ajoute des mines
        Debug.Log($"Ammo pack collected: +{ammoValue} ammo, +{mineValue} mines");
    }
}
