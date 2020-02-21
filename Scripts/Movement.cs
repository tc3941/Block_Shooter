using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float playerX, playerZ;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerX = Input.GetAxis("Horizontal");
        playerZ = Input.GetAxis("Vertical");
        this.gameObject.transform.position = new Vector3(playerX + this.transform.position.x, this.transform.position.y, playerZ + this.transform.position.z);
    }
}
