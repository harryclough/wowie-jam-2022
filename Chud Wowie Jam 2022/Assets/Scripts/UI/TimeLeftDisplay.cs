using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeLeftDisplay : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
    }

    // Function to take a name and time value and update the TMPro text
    public void UpdateTimeLeft(string name, float time)
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text =  name + time.ToString("0.00") +"s";
    }
}
