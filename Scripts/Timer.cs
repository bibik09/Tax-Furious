using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeValue = 90;
    public Text timerText;
    bool stopTime = true;

    //Update is called once per frame
    void Update()
    {
        if (stopTime)
        {
            if (timeValue > 0)

            {
                timeValue -= Time.deltaTime;
            }
            else
            {
                timeValue = 0;
            }
        }

        DisplayTime(timeValue);
    }
    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        //float minutes = Mathf.FloorToInt(timeToDisplay / 60);

        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        float milliseconds = timeToDisplay % 1 * 1000;

        //timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        timerText.text = string.Format("{0:00}:{1:000}", seconds, milliseconds);
    }

    public void StopTime()
    {
        stopTime = false;
    }
}
