using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerEnter : MonoBehaviour
{
    public Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.FindWithTag("Player");
        if (player)
            player.transform.position = position;
    }

}
