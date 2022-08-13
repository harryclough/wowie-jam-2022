using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour, DeathController
{
    public GameObject sprite;
    public Homing homingScript;
    public HealthController healthController;

    public float speed = 4f;

    private SheepController target;
    public SheepController Target
    {
        get { return target; }
        protected set {
            if (target != null)
            {
                target.sheepPickedUpEvent -= OnSheepPickedUp;
            }
            target = value;
            if (target != null)
            {
                homingScript.target = target.gameObject;
                target.sheepPickedUpEvent += OnSheepPickedUp;
            }
            else
            {
                homingScript.target = null;
            }
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }


    public float GetSheepHeuristic(SheepController sheep)
    {
        float distance = Vector3.Distance(sheep.transform.position, transform.position);
        return distance / sheep.targetPriority;
    }

    public abstract SheepController GetBestTarget();

    private void OnSheepPickedUp()
    {
        Target = null;
    }

}