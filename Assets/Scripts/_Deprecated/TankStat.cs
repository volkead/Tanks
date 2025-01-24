using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TankStat
{
    public int health;
    public int ammo;
    public int mineAmmo;

    // Constructeur modifié
    public TankStat(int health, int ammo, int mineAmmo)
    {
        this.health = health;
        this.ammo = ammo;
        this.mineAmmo = mineAmmo;
    }
}
