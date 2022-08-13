using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed = 6f;
    [SerializeField] protected float maxDamage = 50f;
    [SerializeField] protected float minDamage = 25f;
    [SerializeField] protected float lifeTime = 1f;

    protected float lifeTimeRemaining;

    protected Rigidbody2D rb;

    void Start()
    {
        lifeTimeRemaining = lifeTime;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            float damage = Mathf.Lerp(minDamage, maxDamage, lifeTimeRemaining / lifeTime);
            other.GetComponentInParent<HealthController>().Hit(damage);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (lifeTimeRemaining > 0)
        {
            lifeTimeRemaining -= Time.deltaTime;
        }
        if (lifeTimeRemaining <= 0)
        {
            Destroy(gameObject);
        }
    }
}
