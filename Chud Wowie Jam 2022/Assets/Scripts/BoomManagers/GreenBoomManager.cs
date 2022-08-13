using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Derived class with a Boom method for the green sheep
[System.SerializableAttribute]
public class GreenBoomManager : BoomManager
{
    public override void Boom(GameObject gameObject)
    {
        base.Boom(gameObject);
        Debug.Log("Green sheep is boomed");
    }
}
