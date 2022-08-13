using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject sprite;
    public float maxForce = 100f;
    public float maxDistance = 10f;
    public float speed = 5f;
    public float pickupRadius = 3f;

    void Update()
    {
        // Rotate to face the mouse cursor
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = transform.position.z - Camera.main.transform.position.z;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        sprite.transform.rotation = Quaternion.LookRotation(Vector3.forward, mouseWorldPos - transform.position);
    }

    // Function to select sheep under the cursor in the world
    void SelectSheep()
    {
        // Get the mouse position in the world
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = transform.position.z - Camera.main.transform.position.z;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        // Get all game objects with the tag "Sheep" within the maxDistance
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(mouseWorldPos, pickupRadius);
        // Loop through all the game objects with the tag "Sheep"
        for (int i = 0; i < hitColliders.Length; i++)
        {
            // If the game object is a sheep
            if (hitColliders[i].tag == "Sheep")
            {
                // Set the sheep to be the selected sheep
                hitColliders[i].GetComponent<SheepController>().SetSelected(true);
            }
        }
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




        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");
        moveDirection.Normalize();
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}
