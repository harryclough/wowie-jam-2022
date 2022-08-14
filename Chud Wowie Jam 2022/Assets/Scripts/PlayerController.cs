using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject rotatingComponents;

    [Header("Sheep Interaction")]
    [SerializeField] private float maxSheepForce = 100f;
    [SerializeField] private float maxSheepDistance = 10f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float pickupRadius = 3f;

    [SerializeField] private Color fullColor;
    [SerializeField] private Color fadedColor;

    [Header("Guns")]
    public Gun[] guns;
    public int currentGunIndex = 0;

    public delegate void OnGunChangedEvent(int prevGunIndex, int newGunIndex, int nextGunIndex);
    public OnGunChangedEvent onGunChangedEvent;

    void Start()
    {
        onGunChangedEvent?.Invoke(currentGunIndex, currentGunIndex, (currentGunIndex + 1) % guns.Length);
    }
    
    void Update()
    {
        // Rotate to face the mouse cursor
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = transform.position.z - Camera.main.transform.position.z;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        rotatingComponents.transform.rotation = Quaternion.LookRotation(Vector3.forward, mouseWorldPos - transform.position);

        if (Input.GetButtonDown("Shoot"))
        {
            guns[currentGunIndex].TryToFire();
        }
        
        // Toggle gun when "Switch Gun" is pressed
        if (Input.GetButtonDown("Switch Gun"))
        {
            if (guns.Length > 1) 
            {
                guns[currentGunIndex].OnSwitchOff();

                int prevGunIndex = currentGunIndex;
                currentGunIndex = (currentGunIndex + 1) % guns.Length;
                guns[currentGunIndex].OnSwitchTo();

                int nextGunIndex = (currentGunIndex + 1) % guns.Length;
                onGunChangedEvent?.Invoke(prevGunIndex, currentGunIndex, nextGunIndex);
            }
        } else {
            guns[currentGunIndex].UpdateReload();
        }
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
                // hitColliders[i].GetComponent<SheepController>().SetSelected(true);
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
                SheepController sheepController = sheepObject.GetComponent<SheepController>();
                sheepController.ApplyWhistleForce(transform.position, maxSheepForce, 0f, maxSheepDistance);
            }
        }

        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");
        moveDirection.Normalize();
        Vector3 pos = transform.position + moveDirection * speed * Time.deltaTime;

        //Check if player is within the boundaries of circle radius otherwise clamp the player to the circle
        if (pos.magnitude > WaveController.mapRadius)
        {
            pos = pos.normalized * WaveController.mapRadius;
        }
        transform.position = pos;

    }
}
