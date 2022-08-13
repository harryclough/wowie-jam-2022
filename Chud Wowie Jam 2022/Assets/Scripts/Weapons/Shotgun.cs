using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    [Min(2)]
    [SerializeField] protected int pellets = 2;
    [SerializeField] protected float spreadInDegrees = 30f;

    protected override void Fire()
    {
        // Fire the pellets in an even spread
        float deltaAngle = spreadInDegrees / (pellets - 1);
        for (int i = 0; i < pellets; i++)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, deltaAngle * i - spreadInDegrees / 2);
            Instantiate(bulletPrefab, transform.position, transform.rotation * rotation);
        }
    }
}
