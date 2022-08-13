using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour, DeathController
{
    public GameObject sprite;
    public Homing homingScript;
    public HealthController healthController;

    private SheepController target;
    public SheepController Target
    {
        get { return target; }
        private set {
            if (target != null)
            {
                target.onSheepPickedUp -= OnSheepPickedUp;
            }
            target = value;
            target.onSheepPickedUp += OnSheepPickedUp;
        }
    }

    private void OnSheepPickedUp()
    {
        Target = null;
    }

    public abstract void FindNewTarget();

    public void Die()
    {
        Destroy(gameObject);
    }
}
