using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWolfController : EnemyController
{
    private float deltaAngle;

    void Start() {
        if (!Target) {
            Target = GetBestTarget();
        }
    }

    public void FixedUpdate() {
        if (IsCarryingSheep()) {
            float currentSpeed = speed * carriedSheep.enemySlowdown * Time.deltaTime;
            GetComponent<Rigidbody2D>().MovePosition(transform.position + transform.right * currentSpeed);
        }
        else if (Target)
        {
            moveTowardsTarget(speed);
        }
    }

    public override void OnPickUpSheep() {}
}
