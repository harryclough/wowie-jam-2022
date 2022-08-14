using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunUi : MonoBehaviour
{
    public PlayerController player;

    [SerializeField] private Image currentGunIcon;
    [SerializeField] private Image nextGunIcon;
    [SerializeField] private Image bulletIcon;
    [SerializeField] public float bulletOffset;

    private Image[] bulletIcons;

    void Awake()
    {
        player.onGunChangedEvent += OnGunChanged;
        bulletIcon.gameObject.SetActive(false);
    }

    void OnGunChanged(Gun newGun, Gun nextGun)
    {
        currentGunIcon.sprite = newGun.gunIcon;
        nextGunIcon.sprite = nextGun.gunIcon;
        bulletIcon.sprite = newGun.bulletIcon;

        // Create an array of bullet icons by cloning the bullet icon and offsetting it
        bulletIcons = new Image[newGun.maxBullets];
        for (int i = 0; i < newGun.maxBullets; i++)
        {
            bulletIcons[i] = Instantiate(bulletIcon);
            bulletIcons[i].transform.SetParent(bulletIcon.transform.parent);
            bulletIcons[i].transform.localScale = Vector3.one;
            bulletIcons[i].transform.localPosition = new Vector3(bulletIcon.transform.localPosition.x + bulletOffset * i, bulletIcon.transform.localPosition.y, bulletIcon.transform.localPosition.z);
            bulletIcons[i].gameObject.SetActive(true);
        }

    }

    void Update()
    {

        for (int i = 0; i < player.guns[player.currentGunIndex].maxBullets; i++)
        {
            bool isBulletAvailable = i < player.guns[player.currentGunIndex].currentBullets;
            bool isNotReloading = !player.guns[player.currentGunIndex].isReloading();
            bulletIcons[i].gameObject.SetActive(isBulletAvailable && isNotReloading);
        }
    }
}
