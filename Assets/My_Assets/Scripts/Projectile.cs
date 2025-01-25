using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public TankBehavior firerer;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<TankBehavior>(out TankBehavior tank))
        { 
            if(tank != firerer)
            {
                tank.Hit(50);
                Destroy(gameObject);
            }
        }
    }
}
