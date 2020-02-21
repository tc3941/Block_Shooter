using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySetUp : MonoBehaviour
{
    NavMeshAgent meshAgent;
  
    //public Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        //meshAgent = GetComponent<NavMeshAgent>();
        //link = GetComponent<OffMeshLink>();
        //target = Camera.main.GetComponent<PlayerSpawn>().GetPcPosition();
        //link.startTransform = this.transform;
        //target = GameObject.Find("PC");
    }

    // Update is called once per frame
    void Update()
    {
        var target = Camera.main.GetComponent<PlayerSpawn>().GetPcPosition();
        //target = GameObject.Find("PC");
        //link.endTransform= target.transform;
        GetComponent<NavMeshAgent>().SetDestination(target.Value);
    }
}
