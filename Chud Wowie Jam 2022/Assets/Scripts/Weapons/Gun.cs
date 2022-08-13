using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public int maxBullets = 10;
    public float totalFireDelay = 1f;
    public float totalReloadTime = 1f;

    [Header("Bullets")]
    public GameObject bulletPrefab;

    [HideInInspector] public int currentBullets;
    [HideInInspector] public float fireTimer = 0f;
    [HideInInspector] public float reloadTimer = 0f;

    void Start() {
        currentBullets = maxBullets;
    }

    void Update()
    {
        if (reloadTimer > 0)
        {
            reloadTimer -= Time.deltaTime;
        }
        if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }
        if (currentBullets == 0) {
            if (reloadTimer > 0) {
                currentBullets = maxBullets;
            }
            else {
                reloadTimer = totalReloadTime;
            }
        }
    }

    public void TryToFire() {
        if (currentBullets > 0 && fireTimer <= 0 && reloadTimer <= 0) {
            fireTimer = totalFireDelay;
            currentBullets--;
            Fire();
        }
    }

    protected abstract void Fire();
}
