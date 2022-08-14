using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamManager : MonoBehaviour
{
    public float minRoamTimer = 0.5f;
    public float maxRoamTimer = 1.5f;
    public float minForce = 50f;
    public float maxForce = 300f;
    protected float roamTimer = 0.0f;
    protected SheepController sc;
    protected Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        sc = GetComponent<SheepController>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sc.IsTargetable){
            roamTimer -= Time.deltaTime;
            if (roamTimer <= 0.0f){
                Roam();
                roamTimer = Random.Range(minRoamTimer, maxRoamTimer);
            }
        }
    }

    virtual protected void Roam()
    {
        // Generate a random angle to push the sheep in
        float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
        // Generate a random force to push the sheep in
        float force = Random.Range(minForce, maxForce);
        // Apply the force to the sheep
        rb.AddForce(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * force);
    }
}
