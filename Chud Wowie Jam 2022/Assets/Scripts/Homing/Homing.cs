using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Homing : MonoBehaviour
{
    [HideInInspector] public GameObject target;

    protected abstract void LookAt2D();
}
