using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public static HintManager instance { get; private set; }
    public TextMeshProUGUI HintMessage;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        if(HintMessage != null)
            HintMessage.text = string.Empty;
    }

    public void ShowHint(string message)
    {
        if (HintMessage != null)
            HintMessage.text = message;
    }

    public void HideHint()
    {
        if (HintMessage != null)
            HintMessage.text = string.Empty;
    }
}
