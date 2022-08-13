using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Derived class with a Boom method for the yellow sheep
[System.SerializableAttribute]
public class YellowBoomManager : BoomManager
{
    public override void Boom(GameObject gameObject)
    {
        base.Boom(gameObject);
        Debug.Log("Yellow sheep is boomed");
    }
}