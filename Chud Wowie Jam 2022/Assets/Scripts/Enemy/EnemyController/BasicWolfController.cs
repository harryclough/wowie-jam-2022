using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWolfController : EnemyController
{
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
            // Move towrads the target at speed
            Vector3 moveDirection = Target.transform.position - transform.position;
            moveDirection.Normalize();
            GetComponent<Rigidbody2D>().MovePosition(transform.position + moveDirection * speed * Time.deltaTime);
        }
    }

    public override void OnPickUpSheep() {}
}
