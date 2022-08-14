using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public int maxBullets = 10;
    public float totalFireDelay = 1f;
    public float totalReloadTime = 1f;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    public ParticleSystem[] muzzleFlashes;
    public GameObject playerSprite;

    public Sprite gunIcon;
    public Sprite bulletIcon;

    public AudioClip[] fireSounds;
    public AudioSource reloadSource;

    [HideInInspector] public int currentBullets;
    [HideInInspector] public float fireTimer = 0f;
    [HideInInspector] public float reloadTimer = 0f;

    void Awake() {
        currentBullets = maxBullets;
        playerSprite.SetActive(false);
    }

    public void OnSwitchTo() {
        playerSprite.SetActive(true);

        if (currentBullets == 0 || isReloading()) {
            reload();
        } else {
            fireTimer = totalFireDelay;
        }
    }

    public void OnSwitchOff() {
        playerSprite.SetActive(false);
        reloadSource.Stop();
    }

    public void UpdateReload()
    {
        if (Input.GetButtonDown("Reload"))
        {
            currentBullets = 0;
        }

        if (isReloading())
        {
            reloadTimer -= Time.deltaTime;
        }
        else if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }

        if (currentBullets == 0) {
            if (isReloading()) {
                currentBullets = maxBullets;
            }
            else {
                reload();
            }
        }
    }

    public void TryToFire() {
        if (currentBullets > 0 && fireTimer <= 0 && !isReloading()) {
            fireTimer = totalFireDelay;
            currentBullets--;
            Fire();
            if (muzzleFlashes.Length > 0) {
                muzzleFlashes[Random.Range(0, muzzleFlashes.Length)].Play();
            }
            if (fireSounds.Length > 0) {
                // Play a random sound from the array
                AudioSource.PlayClipAtPoint(fireSounds[Random.Range(0, fireSounds.Length)], transform.position, Random.Range(0.85f, 1.15f));
            }
        }
    }

    public bool isReloading() {
        return reloadTimer > 0;
    }

    protected abstract void Fire();

    protected void reload()
    {
        reloadTimer = totalReloadTime;
        fireTimer = 0;
        reloadSource.pitch = Random.Range(0.95f, 1.05f);
        reloadSource.Play();
    }

}
