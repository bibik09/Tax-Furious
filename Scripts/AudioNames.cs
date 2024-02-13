using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioNames : MonoBehaviour
{
    public Text audio1;
    public Text audio2;
    public Text audio3;
    public Text audio4;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (audio1.enabled)
            {
                audio1.enabled = false;
                audio2.enabled = true;
            }
            else if (audio2.enabled)
            {
                audio2.enabled = false;
                audio3.enabled = true;
            }
            else if (audio3.enabled)
            {
                audio3.enabled = false;
                audio4.enabled = true;
            }
            else
            {
                audio4.enabled = false;
                audio1.enabled = true;
            }
        }
    }
}
