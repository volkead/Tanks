using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    public int healValue = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<TankBehavior>(out TankBehavior tank))
        {
            tank.Heal(healValue);
            Destroy(gameObject);
        }
    }

}
