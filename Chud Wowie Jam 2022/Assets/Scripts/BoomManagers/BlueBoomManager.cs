using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Derived class with a Boom method for the blue sheep
[System.SerializableAttribute]
public class BlueBoomManager : BoomManager
{
    [SerializeField] float chainRadius = 2.5f;
    [SerializeField] float speed = 10f;
    private List<Collider2D> chainedEnemies = new List<Collider2D>();
    private GameObject obj;
    private bool chasing;
    private Homing homing;

    public override void Boom(GameObject gameObject)
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        obj = gameObject;
        homing.enabled = true;
        chasing = true;
        Chain(gameObject);
        Debug.Log("Blue sheep is boomed");
    }

    //A recursive function to find the closest enemy to the sheep. If there is an enemy, move the sheep to that enemy and then invoke hitEnemiesInRadius, then repeating on the next closest enemy if it is within chain radius
    void Chain(GameObject gameObject)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, chainRadius);
        hitColliders = SortByDistance(hitColliders);
        int i = 0;
        Debug.Log("HitColliders length: " + hitColliders.Length);
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Enemy" && !chainedEnemies.Contains(hitColliders[i]))
            {
                chainedEnemies.Add(hitColliders[i]);
                homing.target = hitColliders[i].gameObject;
                return;
            }
            i++;
        }
        chasing = false;
        homing.enabled = false;
        chainedEnemies.Clear();
    }

    //A function to sort a array of enemies by distance from the sheep and return the sorted array using bubble sort
    Collider2D[] SortByDistance(Collider2D[] hitColliders)
    {
        for (int i = 0; i < hitColliders.Length; i++)
        {
            for (int j = 0; j < hitColliders.Length - 1; j++)
            {
                if (Vector2.Distance(obj.transform.position, hitColliders[j].transform.position) > Vector2.Distance(obj.transform.position, hitColliders[j + 1].transform.position))
                {
                    Collider2D temp = hitColliders[j];
                    hitColliders[j] = hitColliders[j + 1];
                    hitColliders[j + 1] = temp;
                }
            }
        }
        return hitColliders;
    }


    //A function to move forward until it hits the target enemy and then invoke chain
    public void Chase(GameObject gameObject)
    {
        MoveForward(gameObject);
        //check if the sheep has reached the target enemy
        if (chasing && Vector2.Distance(gameObject.transform.position, homing.target.transform.position) < 0.1f)
        {
            HitEnemiesInRadius(gameObject);
            Chain(gameObject);
        }
    }

    //A function to make the object move forward at a fixed speed in 2D space
    public void MoveForward(GameObject gameObject)
    {
        gameObject.transform.position += gameObject.transform.right * Time.deltaTime * speed;
    }

    private void Start(){
        //create a component Homing to this game object
        homing = gameObject.AddComponent<Homing>();
        homing.enabled = false;
    }

    private void Update() {
        if (chasing){
            Chase(obj);
        }
    }
}



