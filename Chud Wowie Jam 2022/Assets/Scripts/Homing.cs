using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{
    public GameObject target;

    private void FixedUpdate() {
        LookAt2D(target);
    }

    //A function that makes this object look at the target in 2D space so the where forward is up
    void LookAt2D(GameObject target)
    {
        if (!target) {return;}
        Vector2 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    //A function to take a game object and set it as the target
    public void SetTarget(GameObject target)
    {
        if (this.target != null) {
            this.target.GetComponent<EnemyController>().deathEvent -= OnTargetDeath;
            Debug.Log("Unsubbed");
        }
        this.target = target;
        if (target != null) {
            target.GetComponent<EnemyController>().deathEvent += OnTargetDeath;
            Debug.Log("Subbed");
        }
    }

    void OnTargetDeath() {
        target.GetComponent<EnemyController>().deathEvent -= OnTargetDeath;
        Debug.Log("Unsubbed");
        target = null;
    }
}
