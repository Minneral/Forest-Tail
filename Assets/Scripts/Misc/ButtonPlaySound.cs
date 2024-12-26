using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPlaySound : MonoBehaviour
{
    public AudioClip audioClip;

    private void Start()
    {
        Button button = GetComponent<Button>();

        // Добавление слушателя для события onClick
        button.onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        MasterVolume.instance.audioSource.PlayOneShot(audioClip);
    }
}
