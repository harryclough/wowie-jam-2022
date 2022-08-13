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
            else
            {
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
        IsTargetable = true;
        SharedCarryEnd();
    }

    public void PlayerDrop()
    {
        SharedCarryEnd();
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
}
