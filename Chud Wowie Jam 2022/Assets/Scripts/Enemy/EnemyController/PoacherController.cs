using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoacherController : EnemyController
{
    [SerializeField] private float whistleMaxRadius = 10f;
    [SerializeField] private float whistleActivationRadius = 8f;
    [SerializeField] private float whistleForce = 25f;
    [SerializeField] private GameObject whistleAreaIndicator;

    void Start() {
        if (!Target) {
            Target = GetBestTarget();
        }
        whistleAreaIndicator.transform.localScale = Vector3.one * whistleMaxRadius * 2;
        whistleAreaIndicator.SetActive(false);
    }

    public void FixedUpdate() {
        if (IsCarryingSheep()) {
            float currentSpeed = speed * carriedSheep.enemySlowdown * Time.deltaTime;
            GetComponent<Rigidbody2D>().MovePosition(transform.position + transform.right * currentSpeed);
        }
        else if (Target)
        {
            // If the distance to the target is less than the whistle range, whistle!
            if (Vector3.Distance(transform.position, Target.transform.position) < whistleActivationRadius) {
                whistleAreaIndicator.SetActive(true);
                Whistle();
                moveTowardsTarget(speed * Target.enemySlowdown);
            }
            else {
                whistleAreaIndicator.SetActive(false);
                moveTowardsTarget(speed);
            }
        }
    }

    private void Whistle() {
        GameObject[] sheep = GameObject.FindGameObjectsWithTag("Sheep");
        foreach (GameObject sheepObject in sheep)
        {
            SheepController sheepController = sheepObject.GetComponent<SheepController>();
            sheepController.ApplyWhistleForce(transform.position, whistleForce, 0f, whistleMaxRadius);
        }
    }

    public override void OnPickUpSheep()
    {
        whistleAreaIndicator.SetActive(false);
    }
}
