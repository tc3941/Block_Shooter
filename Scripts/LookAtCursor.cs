using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class LookAtCursor : MonoBehaviour
{
    public Entity player;
    EntityManager entityManager;
    // Start is called before the first frame update
    void Start()
    {
        entityManager = World.Active.EntityManager;
        //WaitForPlayerToBeFound();
    }
    IEnumerator WaitForPlayerToBeFound()
    {
        yield return new WaitForEndOfFrame();
        player = GetPlayer();

    }
    Entity GetPlayer()
    {
        return Camera.main.GetComponent<PlayerSpawn>().PC;
    }
    // Update is called once per frame
    void Update()
    {
        if (!PauseSystem.Paused && Camera.main.GetComponent<HpAndTimeScale>().GetAlive())
        {
            var playerLoc = entityManager.GetComponentData<Translation>(player);
            var mousePos = Input.mousePosition;
            var playerPos = Camera.main.WorldToScreenPoint(playerLoc.Value);

            var dir = mousePos - playerPos;

            var angle = Mathf.Atan2(dir.normalized.y, -dir.normalized.x);
            entityManager.SetComponentData<Rotation>(player, new Rotation { Value = quaternion.RotateY(angle - (math.PI / 2)) });
            var dir2 = playerPos - new Vector3(playerLoc.Value.x + 1, playerLoc.Value.y, playerLoc.Value.z);
        }
        //testObject.transform.rotation = Quaternion.AngleAxis(-angle, Vector3.up);

        //Vector3 forward2 = mouseWorld - testObject.transform.position;
        //float3 forward = new float3(mouseWorld.x,mouseWorld.y,mouseWorld.z) - playerLoc;
        //var rot = Quaternion.LookRotation(forward, Vector3.up);
        //var test = entityManager.GetComponentData<Rotation>(player);
        //entityManager.SetComponentData<Rotation>(player, new Rotation { Value = quaternion.LookRotation(forward, new float3(0, 1, 0))});


    }
}
