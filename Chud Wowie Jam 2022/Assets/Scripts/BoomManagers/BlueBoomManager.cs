using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Derived class with a Boom method for the blue sheep
[System.SerializableAttribute]
public class BlueBoomManager : BoomManager
{
    [SerializeField] float chainRadius = 2.5f;
    [SerializeField] float speed = 10f;
    [SerializeField] float hitRadius = 0.1f;
    [SerializeField] BasicHoming homing;
    // private List<Collider2D> chainedEnemies = new List<Collider2D>();
    private HashSet<Collider2D> chainedEnemiesSet = new HashSet<Collider2D>();
    private GameObject obj;
    private bool chasing;

    // Getter for chainradius
    public float ChainRadius {
        get { return chainRadius; }
    }

    public override void Boom(GameObject gameObject)
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        obj = gameObject;
        homing.enabled = true;
        chasing = true;
        Chain(gameObject);
        Debug.Log("Blue sheep is boomed");
    }

    // A recursive function to find the closest enemy to the sheep. If there is an enemy, move the sheep to that enemy and then invoke hitEnemiesInRadius, then repeating on the next closest enemy if it is within chain radius
    void Chain(GameObject gameObject)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, chainRadius);
        hitColliders = SortByDistance(hitColliders);

        // Feel free to use your code but this might be a little cleaner.
        // I would recommended using a HashSet or HashMap instead of a list of chained enemies,
        // since a list.Contains is O(n) whereas map/set.Contains is O(1)
        // (Gotta make my data structures lecturer happy lol)
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.tag == "Enemy" && !chainedEnemiesSet.Contains(collider))
            {
                chainedEnemiesSet.Add(collider);
                homing.SetTarget(collider.gameObject);
                return;
            }
        }
        //If no targets, die
        chainedEnemiesSet.Clear();
        chasing = false;
        homing.SetTarget(null);
        homing.enabled = false;
        Destroy(this.gameObject);
    }

    // A function to sort a array of enemies by distance from the sheep and return the sorted array
    Collider2D[] SortByDistance(Collider2D[] hitColliders)
    {
        // We don't need to manually implement a sorting algorithm if we just List.Sort()
        List<Collider2D> hits = new(hitColliders);
        hits.Sort(delegate(Collider2D lhs, Collider2D rhs) {
            float lhsSqrDist = (transform.position - lhs.transform.position).sqrMagnitude;
            float rhsSqrDist = (transform.position - rhs.transform.position).sqrMagnitude;
            return lhsSqrDist.CompareTo(rhsSqrDist);
        });
        return hits.ToArray();
        /*
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
        */
    }


    // A function to move forward until it hits the target enemy and then invoke chain
    public void Chase(GameObject gameObject)
    {
        MoveForward(gameObject);
        // check if the sheep has reached the target enemy
        if (homing.target == null || (chasing && Vector2.Distance(gameObject.transform.position, homing.target.transform.position) < hitRadius))
        {
            HitEnemiesInRadius(gameObject);
            Chain(gameObject);
        }
    }

    // A function to make the object move forward at a fixed speed in 2D space
    public void MoveForward(GameObject gameObject)
    {
        gameObject.transform.position += gameObject.transform.right * Time.deltaTime * speed;
    }

    private void Start() {
        homing.enabled = false;
    }

    private void Update() {
        if (chasing){
            Chase(obj);
        }
    }
}



