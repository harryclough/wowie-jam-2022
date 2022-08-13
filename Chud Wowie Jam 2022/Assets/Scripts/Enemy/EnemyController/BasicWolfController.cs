using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWolfController : EnemyController
{
    public override SheepController GetBestTarget()
    {
        GameObject[] sheep = GameObject.FindGameObjectsWithTag("Sheep");
        SheepController bestSheep = null;
        float bestValue = float.MaxValue;
        foreach (GameObject sheepObject in sheep)
        {
            float value = GetSheepHeuristic(sheepObject.GetComponent<SheepController>());
            if (value < bestValue)
            {
                bestSheep = sheepObject.GetComponent<SheepController>();
                bestValue = value;
            }
        }
        return bestSheep;
    }

    void Start() {
        if (!Target) {
            Target = GetBestTarget();
        }
    }

    public void FixedUpdate() {
        if (!Target)
        {
            Target = GetBestTarget();
        }
        // Can't use else here since target is could still be null
        if (Target)
        {
            // Move towrads the target at speed
            Vector3 moveDirection = Target.transform.position - transform.position;
            moveDirection.Normalize();
            GetComponent<Rigidbody2D>().MovePosition(transform.position + moveDirection * speed * Time.deltaTime);
        }
    }

    // A method that is called when the wolf collides with a sheep
    void OnCollisionEnter2D(Collision2D collision)
    {
        SheepController sheep = collision.gameObject.GetComponent<SheepController>();
        if (sheep)
        {
            // sheep.GetComponent<Rigidbody2D>().AddForce(-(transform.position - sheep.transform.position).normalized * 1000f);
            sheep.gameObject.SetActive(false);
            sheep.sheepPickedUpEvent();
        }
    }

}
