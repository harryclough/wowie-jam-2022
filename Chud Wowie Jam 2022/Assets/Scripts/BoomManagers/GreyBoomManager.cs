using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Derived class with a Boom method for the grey sheep
[System.SerializableAttribute]
public class GreyBoomManager : BoomManager
{
    public override void Boom(GameObject gameObject)
    {
        base.Boom(gameObject);
        Debug.Log("Grey sheep is boomed");
    }
}
