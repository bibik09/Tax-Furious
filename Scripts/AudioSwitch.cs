using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSwitch : MonoBehaviour
{
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource audioSource3;
    public AudioSource audioSource4;
    void Start()
    {
        audioSource1.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (audioSource1.isPlaying)
            {
                audioSource1.Stop();
                audioSource2.Play();
            }
            else if (audioSource2.isPlaying)
            {
                audioSource2.Stop();
                audioSource3.Play();
            }
            else if (audioSource3.isPlaying)
            {
                audioSource3.Stop();
                audioSource4.Play();
            }
            else
            {
                audioSource4.Stop();
                audioSource1.Play();
            }
        }
    }
}
