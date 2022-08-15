using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    public float pushForce = 1000f;
    void OnTriggerExit2D(Collider2D collision)
    {
        Vector3 pos = collision.gameObject.transform.position;
        if (pos.magnitude < WaveController.mapRadius)
        {
            return;
        }

        Debug.Log("OOB");
        DeathController deathController = collision.gameObject.GetComponent<DeathController>();
        // if the collision object is an enemy and is not holding a sheep, ignore
        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
        if (enemy!=null && !enemy.IsCarryingSheep())
        {
            Debug.Log("Spared Enemy");
            return;
        }

        if (deathController!=null)
        {
            deathController.Die();
            return;
        }
        // If a player (use tag)
        if (collision.gameObject.tag == "Player")
        {
            // Find direction of the player from origin 2d as an angle
            Vector2 direction = collision.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //Place the player at the edge of the radius of the death barrier
            Vector2 newPosition = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * gameObject.transform.localScale.x/2, Mathf.Sin(angle * Mathf.Deg2Rad) * gameObject.transform.localScale.y/2);
            // Set the new position of the player
            collision.transform.position = new Vector3(newPosition.x, newPosition.y, collision.transform.position.z);
        }
    }
}
