using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowHoming : Homing
{
    [SerializeField] protected float rotationSpeed;
    protected void FixedUpdate() {
        LookAt2D();
    }

    protected override void LookAt2D()
    {
        if (!target) {return;}
        Vector2 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.AngleAxis(angle, Vector3.forward),
            rotationSpeed * Time.deltaTime
        );
    }
}