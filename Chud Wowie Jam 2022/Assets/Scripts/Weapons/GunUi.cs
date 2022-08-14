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

    [SerializeField] private Color bulletFadedColor;

    private Color bulletFullColor = Color.white;

    private List<Image[]> bulletIcons;

    void Awake()
    {
        player.onGunChangedEvent += OnGunChanged;
        bulletIcon.gameObject.SetActive(false);

        // For each gun, create an array of bullet icons
        bulletIcons = new List<Image[]>();
        foreach (Gun gun in player.guns)
        {
            bulletIcon.sprite = gun.bulletIcon;
            Image[] icons = new Image[gun.maxBullets];
            for (int i = 0; i < gun.maxBullets; i++)
            {
                icons[i] = Instantiate(bulletIcon);
                icons[i].transform.SetParent(bulletIcon.transform.parent);
                icons[i].transform.localScale = Vector3.one;
                icons[i].transform.localPosition = bulletIcon.transform.localPosition + new Vector3(bulletOffset * i, 0, 0);
            }
            bulletIcons.Add(icons);
        }
    }

    void OnGunChanged(int prevGunIndex, int currentGunIndex, int nextGunIndex)
    {
        foreach (Image icons in bulletIcons[prevGunIndex])
        {
            icons.gameObject.SetActive(false);
        }
        currentGunIcon.sprite = player.guns[currentGunIndex].gunIcon;
        nextGunIcon.sprite = player.guns[nextGunIndex].gunIcon;
        foreach (Image icons in bulletIcons[currentGunIndex])
        {
            icons.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        Gun gun = player.guns[player.currentGunIndex];
        for (int i = 0; i < gun.maxBullets; i++)
        {
            bool active = i < gun.currentBullets && !gun.isReloading();
            Color color = active ? bulletFullColor : bulletFadedColor;
            bulletIcons[player.currentGunIndex][i].color = color;
        }
    }
}
