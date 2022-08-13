using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed = 6f;
    [SerializeField] protected float damage = 50f;
    [SerializeField] protected float lifeTime = 1f;

    protected Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponentInParent<HealthController>().Hit(damage);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        // Destroy the bullet after it's lifetime
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
