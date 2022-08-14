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
            rb.MovePosition(transform.position + transform.right * currentSpeed);
        }
        else if (Target)
        {
            moveTowardsTarget(speed);
        }
        else
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            moveTowardsPosition(player.transform.position, speed);
        }
    }

    public override void OnPickUpSheep() {}
}
