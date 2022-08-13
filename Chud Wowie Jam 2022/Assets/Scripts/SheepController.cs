using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : MonoBehaviour
{
    // Bootmanager variable
    public BoomManager boomManager;

    public delegate void SheepPickedUpEvent();
    public SheepPickedUpEvent sheepPickedUpEvent;

    public float targetPriority = 1f;

    // Boom method
    public void Boom()
    {
        boomManager.Boom(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Boom();
        }
    }
}
