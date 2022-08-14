using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineController : EnemyController
{

    void Start() {
        if (!Target) {
            Target = GetBestTarget();
        }
    }

    public void FixedUpdate() {
        if (Target)
        {
            moveTowardsTarget(speed);
        }
    }

    public override void OnPickUpSheep() {}

    protected override void OnCollisionEnter2D(Collision2D collision) {}
}
