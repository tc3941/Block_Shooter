using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerSpawn playerSpawnScript;
    public float x, y, z;
    void Start()
    {
        x = this.transform.position.x; y = this.transform.position.y; z = this.transform.position.z;
        playerSpawnScript = GetComponent<PlayerSpawn>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerSpawnScript.PC != null)
        {
            var PcPos = playerSpawnScript.GetPcPosition().Value;
            this.transform.position = new Vector3(PcPos.x + x, PcPos.y + y, PcPos.z + z);
        }
        else
            print("Player not found");
    }
}
