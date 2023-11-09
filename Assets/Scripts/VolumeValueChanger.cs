using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeValueChanger : MonoBehaviour
{
    private AudioSource audioSrc;
    private float musicVolume = 1f;
 

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();

        if (audioSrc == null)
        {
            Debug.LogError("No AudioSource component found on this GameObject");
        }
    }

    void Update()
    {
        if (audioSrc != null)
        {
            audioSrc.volume = musicVolume;
        }
        else
        {
            Debug.LogWarning("audioSrc is null in Update.");
        }
    }

    public void SetVolume(float vol)
    {
        musicVolume = vol;
    }
}
