using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Set position of the gameobject to the mouse's current position in the world using the camera's position and the mouse's position in the screen
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        transform.position = pos;
    }

    // A method to set the x and y scale of the crosshair, double the value provided
    public void SetScale(float scale)
    {
        transform.localScale = new Vector3(scale*2, scale*2, 1);
    }

    // A method to set the colour of the crosshair to the colour provided
    public void SetColour(Color colour)
    {
        GetComponent<SpriteRenderer>().color = colour;
    }
}
