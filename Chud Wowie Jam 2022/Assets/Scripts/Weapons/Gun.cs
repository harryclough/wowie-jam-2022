using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public int maxBullets = 10;
    public float totalFireDelay = 1f;
    public float totalReloadTime = 1f;

    public GameObject bulletPrefab;

    public ParticleSystem[] muzzleFlashes;

    [HideInInspector] public int currentBullets;
    [HideInInspector] public float fireTimer = 0f;
    [HideInInspector] public float reloadTimer = 0f;

    void Start() {
        currentBullets = maxBullets;
    }

    public void OnSwitchTo() {
        if (currentBullets == 0) {
            reloadTimer = totalReloadTime;
        } else {
            fireTimer = totalFireDelay;
        }
    }

    public void OnSwitchOff() { }

    public void UpdateReload()
    {
        if (reloadTimer > 0)
        {
            reloadTimer -= Time.deltaTime;
        }
        else if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }

        if (currentBullets == 0) {
            if (reloadTimer > 0) {
                currentBullets = maxBullets;
            }
            else {
                reloadTimer = totalReloadTime;
                fireTimer = 0;
            }
        }
    }

    public void TryToFire() {
        if (currentBullets > 0 && fireTimer <= 0 && reloadTimer <= 0) {
            fireTimer = totalFireDelay;
            currentBullets--;
            Fire();
            if (muzzleFlashes.Length > 0) {
                muzzleFlashes[Random.Range(0, muzzleFlashes.Length)].Play();
            }
        }
    }

    protected abstract void Fire();

}
