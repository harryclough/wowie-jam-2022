using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject sprite;
    public float maxForce = 100f;
    public float maxDistance = 10f;
    public float speed = 5f;

    void Update()
    {
        // Rotate to face the mouse cursor
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = transform.position.z - Camera.main.transform.position.z;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        sprite.transform.rotation = Quaternion.LookRotation(Vector3.forward, mouseWorldPos - transform.position);
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            GameObject[] sheep = GameObject.FindGameObjectsWithTag("Sheep");
            foreach (GameObject sheepObject in sheep)
            {
                Vector3 forceDirection = transform.position - sheepObject.transform.position;
                forceDirection.Normalize();
                // The force is maxForce at 0 distance and 0 force at maxDistance
                float distance = Vector3.Distance(sheepObject.transform.position, transform.position);
                float forceAmount = Mathf.Lerp(maxForce, 0f, distance / maxDistance);
                sheepObject.GetComponent<Rigidbody2D>().AddForce(forceDirection * forceAmount);
            }
        }

        // The player moves in the direction that is input at their speed
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Vector3.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += Vector3.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector3.right;
        }
        moveDirection.Normalize();
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}
