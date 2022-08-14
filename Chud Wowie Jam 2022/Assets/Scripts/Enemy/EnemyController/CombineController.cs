using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineController : EnemyController
{
    public float suctionForce = 30f;
    public float suctionRadius = 8f;

    void Start() {
        if (!Target) {
            Target = GetBestTarget();
        }
    }

    public void FixedUpdate() {
        if (Target)
        {
            moveTowardsTarget(speed);
            Target.ApplyWhistleForce(transform.position, suctionForce, 0f, suctionRadius);
        }
        else
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float closestDistance = float.MaxValue;
            GameObject closestEnemy = null;
            foreach (GameObject enemy in enemies)
            {
                if (gameObject != enemy)
                {
                    float distance = (transform.position - enemy.transform.position).sqrMagnitude;
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = enemy;
                    }
                }
            }
            if (closestEnemy != null)
            {
                moveTowardsPosition(closestEnemy.transform.position, speed);
                homingScript.target = closestEnemy;
            }
        }
    }

    public override void OnPickUpSheep() {}

    protected override void OnCollisionEnter2D(Collision2D collision) {}
}
