using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    public float pushForce = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // On collision exit, destroy the gameobject, invoke die if it has a deathcontroller
    void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the objec is actually OOB (Not just turned collisions off)
        Vector3 pos = collision.gameObject.transform.position;
        if (pos.magnitude < WaveController.mapRadius)
        {
            return;
        }

        Debug.Log("OOB");
        DeathController deathController = collision.gameObject.GetComponent<DeathController>();
        if (deathController!=null)
        {
            deathController.Die();
            return;
        }
        // If a player (use tag)
        if (collision.gameObject.tag == "Player")
        {
            /*
            // Find direction of the player from origin 2d as an angle
            Vector2 direction = collision.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //Place the player at the edge of the radius of the death barrier
            Vector2 newPosition = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * gameObject.transform.localScale.x/2, Mathf.Sin(angle * Mathf.Deg2Rad) * gameObject.transform.localScale.y/2);
            // Set the new position of the player
            collision.transform.position = new Vector3(newPosition.x, newPosition.y, collision.transform.position.z);
            */
            /*
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 force = rb.transform.position.normalized;
            rb.AddForce(-force * pushForce);
            Debug.Log("Pushed player "+force);
            */
        }
    }
}
