using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public bool isDead = false;

    public void Hit(float damage)
    {
        Debug.Log("Hit!");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();

        }
    }

    public void Die()
    {
        //TODO: Add death walk
        gameObject.SetActive(false);
    }
}
