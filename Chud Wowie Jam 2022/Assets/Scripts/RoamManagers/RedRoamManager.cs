using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedRoamManager : RoamManager
{
    public float fleeRange = 10.0f;

    override protected void Roam(){
        // Find all nearby characters radius using a collider exluding
        Collider2D[] nearbyCharacters = Physics2D.OverlapCircleAll(transform.position, fleeRange);
        if (nearbyCharacters.Length == 0){
            Debug.Log("Using base");
            base.Roam();
        }
        else{
        //Find the closest character to the sheep
        float closestDistance = Mathf.Infinity;
        Collider2D closestCharacter = null;
        foreach (Collider2D character in nearbyCharacters){
            if (character.gameObject == gameObject){
                continue;
            }
            float distance = Vector2.Distance(transform.position, character.transform.position);
            if (distance < closestDistance){
                closestDistance = distance;
                closestCharacter = character;
            }
        }
        // find the direction away from the closest character
        Vector2 direction = (transform.position - closestCharacter.transform.position).normalized;
        // Generate a random force to push the sheep in
        float force = Random.Range(minForce, maxForce);
        // Apply the force to the sheep
        rb.AddForce(direction * force);
        Debug.Log("Flee: "+direction);
        }
    }
}
