using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject rotatingComponents;
    [SerializeField] private CrosshairController crosshair;
    public Transform pickedUpSheepPosition;

    [Header("Sheep Interaction")]
    [SerializeField] private float maxSheepForce = 100f;
    [SerializeField] private float maxSheepDistance = 10f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float pickupRadius = 3f;

    [Header("Guns")]
    public Gun[] guns;
    public int currentGunIndex = 0;

    [HideInInspector] public SheepController carriedSheep = null;

    public delegate void OnGunChangedEvent(int prevGunIndex, int newGunIndex, int nextGunIndex);
    public OnGunChangedEvent onGunChangedEvent;
    private bool pressedPickup = false;

    void Start()
    {
        onGunChangedEvent?.Invoke(currentGunIndex, currentGunIndex, (currentGunIndex + 1) % guns.Length);
        guns[currentGunIndex].playerSprite.SetActive(true);
        guns[currentGunIndex].reloadSource.Play();
        UpdateCrosshair(null);
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
            if (carriedSheep == null){
                guns[currentGunIndex].TryToFire();
            }
            else {
                throwSheep();
            }
        }

        if (Input.GetButtonDown("Pickup Sheep")){
            pressedPickup = true;
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
                SheepController sheep = hitColliders[i].GetComponent<SheepController>();
                // If the sheep is not already being carried
                if (sheep.IsTargetable)
                {
                    // Get the direction from the player to the sheep
                    Vector3 direction = sheep.transform.position - transform.position;
                    // Get the distance from the player to the sheep
                    float distance = direction.magnitude;
                    // If the distance is less than the max distance
                    if (distance < pickupRadius)
                    {
                        sheep.PlayerPickUp(pickedUpSheepPosition);
                        carriedSheep = sheep;
                        UpdateCrosshair(sheep);     
                    }
                }
                return;
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

        if (pressedPickup)
        {
            pressedPickup = false;
            if (carriedSheep == null)
            {
                SelectSheep();
            }
            else{
                carriedSheep.PlayerDrop();
                carriedSheep = null;
                UpdateCrosshair(null);
            }
        }

        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");
        moveDirection.Normalize();
        float sheepSlow = 1f;
        if (carriedSheep != null)
        {
            sheepSlow = carriedSheep.enemySlowdown;
        }
        Vector3 pos = transform.position + moveDirection * speed * sheepSlow * Time.deltaTime;
        //Check if pos is within the radius of the map using the map bounds
        if (pos.magnitude > WaveController.mapRadius){
            //clamp the player to the circle of the map
            pos = pos.normalized * WaveController.mapRadius;
        }
        transform.position = pos;
    

    }

    public void UpdateCrosshair(SheepController sheep){
        if (sheep == null){ //If it's a gun now, set the crosshair to be the size of the gun
            crosshair.SetColour(Color.black);
            crosshair.SetScale(0.2f);
            return;
        }
        // If sheep has a BlueBoomManager, set the scale based on the chain radius, otherwise use the blast radius
        //set colour of crosshair's sprite to the sheep's colour
        crosshair.SetColour(sheep.colour);
        if (sheep.GetComponent<BlueBoomManager>())
        {
            crosshair.SetScale(sheep.GetComponent<BlueBoomManager>().ChainRadius);
        }
        else{ //Any other sheep
            crosshair.SetScale(sheep.GetComponent<BoomManager>().BlastRadius); 
        }
    }

    // Drop sheep and activate sheep throw with the position of the mouse in world space
    void throwSheep()
    {
        carriedSheep.PlayerDrop();
        carriedSheep.Throw(crosshair.transform.position);
        carriedSheep = null;
        UpdateCrosshair(null);
    }
}
