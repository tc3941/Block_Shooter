using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
public class PlayerSpawn : MonoBehaviour
{
    public GameObject PcPrefab,spawnPoint;
    public Entity PC;
    EntityManager entityManager;
    // Start is called before the first frame update
    void Start()
    {


        var pc = GameObjectConversionUtility.ConvertGameObjectHierarchy(PcPrefab, World.Active);
        entityManager = World.Active.EntityManager;

        var player = entityManager.Instantiate(pc);
        PC = player;
        GetComponent<LookAtCursor>().player = PC;
        GetComponent<GameWorldVars>().player = PC;
        if (GetComponent<HpAndTimeScale>() != null)
        {
            GetComponent<HpAndTimeScale>().player = PC;
            GetComponent<HpAndTimeScale>().GetReferences();
        }
        entityManager.SetComponentData(PC, new Translation { Value = spawnPoint.transform.position });
    }

    // Update is called once per frame
    void Update()
    {
       
       // PC = player;
       
    }
    public Translation GetPcPosition()
    {
        return entityManager.GetComponentData<Translation>(PC);
    }
}
