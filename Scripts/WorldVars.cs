using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldVars : MonoBehaviour
{
    public GameObject player;
    GameObject Player{
        get
        {
            return player;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player == null)
        {
            player = GameObject.Find("PC");
        }
    }
}
