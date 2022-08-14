using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SheepMarker : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] SheepController sheep;
    [SerializeField] GameObject marker;
    [SerializeField] GameObject alert;
    [SerializeField] GameObject sprite;


    // Start is called before the first frame update
    void Start()
    {
        //set marker colour to match the sprite colour
        marker.GetComponent<Image>().color = sprite.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the sheep is off screen, if not, show a marker
        if (!IsOnScreen())
        {
            // Show the marker
            canvas.enabled = true;
            marker.SetActive(true);
            if (sheep.IsTargetable){ // If sheep has been grabbed
                alert.SetActive(false);
            }
            else{
                alert.SetActive(true);
            }
        }
        else
        {
            // If it's on screen, hide the marker
            Debug.Log("OFFSCREEN");
            canvas.enabled = false;
            marker.SetActive(false);
            return;
        }

        // Set position to be the centre of the screen top down 2d
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        // Find the angle between pos and the sheep's position
        float angle = Mathf.Atan2(sheep.transform.position.y - pos.y, sheep.transform.position.x - pos.x) * Mathf.Rad2Deg;
        // Rotate the marker to match the angle
        marker.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Get the rectransform of the marker
        RectTransform rt = marker.GetComponent<RectTransform>();
        // Get the x and y of the angle on a circle of radius the canvas width
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * canvas.GetComponent<RectTransform>().rect.width / 2.5f;
        float y = Mathf.Sin(angle * Mathf.Deg2Rad) * canvas.GetComponent<RectTransform>().rect.height / 2.5f;
        rt.localPosition = new Vector3(x, y, 0);
        rt = alert.GetComponent<RectTransform>();
        rt.localPosition = new Vector3(x, y, 0);



    }


    // Function to check if the sheep is within the boundaries of the screen in 2D
    bool IsOnScreen()
    {
        // Get the screen's top left and bottom right corners in the 2d world
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        Vector3 bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        Debug.Log(topLeft.x + " " + topLeft.y + " " + bottomRight.x + " " + bottomRight.y);
        Debug.Log(sheep.transform.position.x + " " + sheep.transform.position.y);
        // Check if the sheep is within the screen
        if (sheep.transform.position.x > topLeft.x && sheep.transform.position.x < bottomRight.x && sheep.transform.position.y < topLeft.y && sheep.transform.position.y > bottomRight.y)
        {
            Debug.Log("True");
            return true;
        }
        else
        {
            return false;
        }
    }
}
