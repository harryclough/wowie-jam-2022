using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MusicController : MonoBehaviour
{
    void Awake()
    {
        GameObject[] musicControllers = GameObject.FindGameObjectsWithTag("MusicController");
        Debug.Log("Found " + musicControllers.Length + " music controllers.");
        if (musicControllers.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
