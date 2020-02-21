using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forceMaterial : MonoBehaviour
{
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<MeshRenderer>().material = mat;
        GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //GetComponent<MeshRenderer>().material = mat;
    }
}
