using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : MonoBehaviour
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
    public bool IsTargetable
    {
        get { return isTargetable; }
        private set
        {
            isTargetable = value;
            if (isTargetable){
                AlertEnemiesTargetable();
            }
            else if (onSheepUntargetableEvent != null){
                onSheepUntargetableEvent(this);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Boom();
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Die()
    {
        isTargetable = false;
        Destroy(gameObject);
    }

    // Boom method
    public void Boom()
    {
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
        PickUp(newParent);
    }

    public void PlayerPickUp(Transform newParent)
    {
        if (!isTargetable)
        {
            Debug.LogWarning("Tried to pick up sheep that is already not targetable!");
            return;
        }
        PickUp(newParent);
    }

    public void EnemyDrop()
    {
        IsTargetable = true;
        PutDown();
    }

    public void PlayerDrop()
    {
        PutDown();
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

    private void PickUp(Transform newParent)
    {
        rb.simulated = false;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        gameObject.transform.parent = newParent;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        gameObject.transform.localScale = Vector3.one;
    }

    private void PutDown()
    {
        rb.simulated = true;

        gameObject.transform.parent = null;
        gameObject.transform.localScale = Vector3.one;
    }
}
