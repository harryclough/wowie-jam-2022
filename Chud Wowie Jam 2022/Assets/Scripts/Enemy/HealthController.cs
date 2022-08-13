using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float maxHealth = 100f;
    [HideInInspector] public float currentHealth = 100f;

    public void Hit(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            gameObject.GetComponent<DeathController>().Die();
        }
    }
}
