using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Derived class with a Boom method for the red sheep
[System.SerializableAttribute]
public class RedBoomManager : BoomManager
{
    public override void Boom(GameObject gameObject)
    {
        base.Boom(gameObject);
        Debug.Log("Red sheep is boomed");
    }
}
