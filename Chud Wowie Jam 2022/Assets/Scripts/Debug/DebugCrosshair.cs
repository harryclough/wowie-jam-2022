using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCrosshair : MonoBehaviour
{
    public GameObject sheep;
    public CrosshairController cc;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        cc.SetColour(sheep.GetComponent<SpriteRenderer>().color);
        
        // If sheep has a BlueBoomManager, set the scale based on the chain radius, otherwise use the blast radius
        if (sheep.GetComponent<BlueBoomManager>())
        {
            cc.SetScale(sheep.GetComponent<BlueBoomManager>().ChainRadius);
        }
        else{
            cc.SetScale(sheep.GetComponent<BoomManager>().BlastRadius); 
        }
    }
}
