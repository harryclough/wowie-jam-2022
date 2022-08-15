using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Base class with a Boom method to be overriden by children
[System.Serializable]
public class BoomManager : MonoBehaviour {
    [SerializeField] float blastRadius = 2.5f; 
    [SerializeField] float blastDamage = 100f;
    [HideInInspector] public ParticleSystem boomParticles;
    protected float floorTime = 1f;
    public float crosshair = 0;

    // Start is called before the first frame update
    void Start()
    {
        boomParticles = GetComponent<ParticleSystem>();
        crosshair = blastRadius;
    }

    // Getter for blastRadius
    public float BlastRadius {
        get { return blastRadius; }
    }

    // Boom method to be overriden by children
    public virtual void Boom(GameObject gameObject){
        HitEnemiesInRadius(gameObject);
        Debug.Log("Boom!");
        StartCoroutine(DestroyAfter());
    }

    // Subroutine to destroy the object after 1 second
    IEnumerator DestroyAfter() {
        yield return new WaitForSeconds(floorTime);
        Destroy(gameObject);
    }

    // A function to find all game objects with the tag "Enemy" within the 2D blast radius and invoke hit on them in their HealthController script for blast damage
    public void HitEnemiesInRadius(GameObject gameObject){
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, blastRadius);
        
        //set the particle system to emit from the gameobject's position


        //Set size of boom particles to blast radius
        if (GetComponent<BlueBoomManager>()) {
            GetComponent<ParticleSystemRenderer>().lengthScale = GetComponent<BlueBoomManager>().ChainRadius;
        }
        else{
            GetComponent<ParticleSystemRenderer>().lengthScale = blastRadius;
        }
        //set colour of boom particles to be the colour from the sheep controller
        var main = GetComponent<ParticleSystem>().main;
        // set particlesystemrenderer map colour
        GetComponent<ParticleSystemRenderer>().material.color = gameObject.GetComponent<SheepController>().colour;
        main.startSize = blastRadius*2;

        //play the particle system
        GetComponent<ParticleSystem>().Play();

        int i = 0;
        //Debug.Log("HitColliders length: " + hitColliders.Length);
        while (i < hitColliders.Length) {
            if (hitColliders[i].tag == "Enemy") {
                hitColliders[i].GetComponent<HealthController>().Hit(blastDamage);
            }
            i++;
        }
    }
}

