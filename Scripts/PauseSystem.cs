using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    static bool pause;

    public static bool Paused
    {
        get
        {
            return pause;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
            TogglePause();
    }

    void TogglePause()
    {
        print("Pause or unpaused");
        pause = !pause;
    }
}
