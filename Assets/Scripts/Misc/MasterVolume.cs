using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterVolume : MonoBehaviour
{
    public static MasterVolume instance { get; private set; }
    public AudioSource audioSource { get; private set; }
    public AudioClip birdClip { get; private set; }
    public AudioClip caveClip { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
