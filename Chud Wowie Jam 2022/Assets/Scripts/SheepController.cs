using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : MonoBehaviour, DeathController
{
    // Bootmanager variable
    public BoomManager boomManager;

    public delegate void OnSheepUntargetableEvent(SheepController sheep);
    public OnSheepUntargetableEvent onSheepUntargetableEvent;

    [HideInInspector] public Rigidbody2D rb;

    public float targetPriority = 1f;

    public float enemySlowdown = 0.25f;
    public float whistleForceMultiplier = 1f;

    private bool isTargetable = true;
    private bool isThrown = false;
    private Vector3 thrownTarget;
    public float throwSpeed = 10f;

    public Color colour;

    public AudioSource[] pickupSounds;

    public bool IsTargetable{
        get { return isTargetable; }
        private set
        {
            isTargetable = value;
            if (isTargetable)
            {
                AlertEnemiesTargetable();
            }
            else if (onSheepUntargetableEvent != null)
            {
                onSheepUntargetableEvent(this);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Boom();
        }
    }

    void FixedUpdate()
    {
        if (isThrown){
            rb.velocity = (thrownTarget - transform.position).normalized * throwSpeed;
            //if reached target position, boom
            if (Vector3.Distance(transform.position, thrownTarget) < 0.1f)
            {
                Boom();
                isThrown = false;
            }
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Boom method
    public void Boom()
    {
        isTargetable = false;
        boomManager.Boom(gameObject);
    }

    public void EnemyPickUp(Transform newParent)
    {
        if (!isTargetable)
        {
            Debug.LogWarning("Tried to pick up sheep that is already not targetable!");
            return;
        }
        IsTargetable = false;
        SharedPickUp(newParent);
        if (pickupSounds.Length > 0)
        {
            AudioSource pickupSound = pickupSounds[Random.Range(0, pickupSounds.Length)];
            pickupSound.pitch = Random.Range(0.9f, 1.25f);
            pickupSound.Play();
        }
    }

    public void PlayerPickUp(Transform newParent)
    {
        if (!isTargetable)
        {
            Debug.LogWarning("Tried to pick up sheep that is already not targetable!");
            return;
        }
        SharedPickUp(newParent);
    }

    public void EnemyDrop()
    {
        // Check if dropped out of bounds of the ring
        if (transform.position.magnitude > WaveController.mapRadius)
        {
            Die();
        }
        IsTargetable = true;
        SharedCarryEnd();
    }

    public void PlayerDrop()
    {
        SharedCarryEnd();
    }

    public void ApplyWhistleForce(Vector3 sourcePos, float maxForce, float minForce, float maxDistance)
    {
        Vector3 forceDirection = sourcePos - transform.position;
        forceDirection.Normalize();
        float distance = Vector3.Distance(sourcePos, transform.position);
        float forceAmount = Mathf.Lerp(maxForce, minForce, distance / maxDistance);
        rb.AddForce(forceDirection * forceAmount);
    }

    private void AlertEnemiesTargetable()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyController>().OnNewTargetAvailable(this);
        }
    }

    private void SharedPickUp(Transform newParent)
    {
        rb.simulated = false;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        gameObject.transform.parent = newParent;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        gameObject.transform.localScale = Vector3.one;
    }

    private void SharedCarryEnd()
    {
        rb.simulated = true;

        gameObject.transform.parent = null;
        gameObject.transform.localScale = Vector3.one;
    }

    //Implement deathcontroller die
    public void Die()
    {
        IsTargetable = false;
        if (GetComponent<BlueBoomManager>()){
            GetComponent<BlueBoomManager>().Die();
            return;
        }
        Destroy(gameObject);
    }

    public void Throw(Vector3 target){
        isThrown = true;
        rb.simulated = true;
        GetComponent<CircleCollider2D>().enabled = false;
        // rotate to face the target in 2d
        thrownTarget = target;
        Vector2 target2d = new Vector2(target.x, target.y);
        Vector2 sheep2d = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = target2d - sheep2d;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}