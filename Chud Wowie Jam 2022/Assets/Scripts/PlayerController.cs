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
        if (Input.GetAxis("Call Sheep") > 0)
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

        // The player moves in the direction of the input axes
        // for both horizontal and veritcal axes
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");
        moveDirection.Normalize();
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}
