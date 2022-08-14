using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    protected override void Fire()
    {
        Instantiate(bulletPrefab, bulletSpawnPoint.position, transform.rotation);
    }
}
