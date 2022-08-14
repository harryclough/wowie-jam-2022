using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineBlades : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        string tag = collider.gameObject.tag;
        if (tag == "Sheep")
        {
            collider.gameObject.GetComponent<SheepController>().Die();
        }
        else if (tag == "Enemy")
        {
            collider.gameObject.GetComponent<EnemyController>().Die();
        }
    }
}
