using System;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private LayerMask targets;
    [SerializeField] private float damage;
    [SerializeField] private float duration;
    [SerializeField] private bool isPoisonous;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targets) != 0)
        {
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable != null)
            {
                if (isPoisonous)
                {
                    damageable.PoisenDamage(damage, duration);
                }
                else
                {
                    damageable.TakeDamage(damage);
                }
            }
        }
    }
}
