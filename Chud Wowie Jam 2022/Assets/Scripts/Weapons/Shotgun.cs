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
            float angle = -spreadInDegrees / 2 + i * deltaAngle;
            angle += Random.Range(-deltaAngle / 2, deltaAngle / 2);
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Instantiate(bulletPrefab, transform.position, transform.rotation * rotation);
        }
    }
}
