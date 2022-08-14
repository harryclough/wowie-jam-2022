using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour, DeathController
{
    public GameObject sprite;
    public Homing homingScript;
    public HealthController healthController;
    public Transform pickedUpSheepPosition;

    public float speed = 4f;

    [HideInInspector] public SheepController carriedSheep = null;

    protected SheepController target;
    public SheepController Target
    {
        get { return target; }
        protected set {
            if (target != null)
            {
                target.onSheepUntargetableEvent -= OnTargetUntargetable;
            }
            target = value;
            if (target != null)
            {
                homingScript.target = target.gameObject;
                target.onSheepUntargetableEvent += OnTargetUntargetable;
            }
            else
            {
                homingScript.target = null;
            }
        }
    }

    public void Die()
    {
        if (IsCarryingSheep()) {
            carriedSheep.EnemyDrop();
        }
        if (Target) {
            target.onSheepUntargetableEvent -= OnTargetUntargetable;
        }
        Destroy(gameObject);
    }

    public void OnNewTargetAvailable(SheepController newTarget)
    {
        if (!IsCarryingSheep())
        {
            Target = GetBestTarget();
        }
    }

    public SheepController GetBestTarget()
    {
        GameObject[] sheep = GameObject.FindGameObjectsWithTag("Sheep");
        SheepController bestSheep = null;
        float bestValue = float.MaxValue;
        foreach (GameObject sheepObject in sheep)
        {
            SheepController sheepController = sheepObject.GetComponent<SheepController>();
            if (sheepController && sheepController.IsTargetable)
            {
                float value = GetSheepHeuristic(sheepObject.GetComponent<SheepController>());
                if (value < bestValue)
                {
                    bestSheep = sheepObject.GetComponent<SheepController>();
                    bestValue = value;
                }
            }
        }
        return bestSheep;
    }

    public bool IsCarryingSheep()
    {
        return carriedSheep != null;
    }

    public abstract void OnPickUpSheep();

    protected float GetSheepHeuristic(SheepController sheep)
    {
        float distance = Vector3.Distance(sheep.transform.position, transform.position);
        return distance / sheep.targetPriority;
    }

    protected void OnTargetUntargetable(SheepController sheep)
    {
        if (!IsCarryingSheep() && sheep == Target)
        {
            Target = GetBestTarget();
        }   
    }

    protected void moveTowardsTarget(float speed) {
        Vector3 moveDirection = target.transform.position - transform.position;
        moveDirection.Normalize();
        GetComponent<Rigidbody2D>().MovePosition(transform.position + moveDirection * speed * Time.deltaTime);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        SheepController sheep = collision.gameObject.GetComponent<SheepController>();
        if (sheep && !IsCarryingSheep() && sheep.IsTargetable)
        {
            carriedSheep = sheep;
            sheep.EnemyPickUp(pickedUpSheepPosition);

            Vector3 direction = transform.position.normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Target = null;
            OnPickUpSheep();
        }
    }

    public void OnWaveEnd(){
        // Run away

    }
}

