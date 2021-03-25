using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeChanger : MonoBehaviour
{
    private AudioSource src;
    private float volume1 = 1f;
    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        src.volume = volume1;    
    }

    public void setVolume(float vol)
    {
        volume1 = vol;
    }
}
