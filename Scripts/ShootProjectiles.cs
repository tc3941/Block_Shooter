using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class ShootProjectiles : MonoBehaviour
{
    Translation PC;
    EntityManager entityManager;
    float timer = 0;
    float delay;
    // Start is called before the first frame update
    void Start()
    {
        entityManager = World.Active.EntityManager;
    }

    // Update is called once per frame
    void Update()
    {
        PC = UnityEngine.Camera.main.GetComponent<PlayerSpawn>().GetPcPosition();

        var player = Camera.main.GetComponent<PlayerSpawn>().PC;

        delay = GameWorldVars.shotDelay;

       // delay = Camera.main.GetComponent<GameWorldVars>().shotDelay;

        var shot = Input.GetButton("Fire1");
        if (timer > 0)
            timer -= Time.deltaTime;
        else
            timer = 0;


        if (shot && timer <= 0&&!PauseSystem.Paused&&Camera.main.GetComponent<HpAndTimeScale>().GetAlive())
        {

            Debug.Log("clicked");
            var rotation = entityManager.GetComponentData<Rotation>(player);

            var proj = entityManager.Instantiate(Camera.main.GetComponent<GameWorldVars>().GetProjectile());
            entityManager.SetComponentData<Translation>(proj, new Translation { Value = new float3(PC.Value.x + math.forward(rotation.Value).x, PC.Value.y + math.forward(rotation.Value).y, PC.Value.z + math.forward(rotation.Value).z) });
            timer = delay;

            var playerComp = entityManager.GetComponentData<ShootProjRunTime>(player);
            var currRot = entityManager.GetComponentData<Rotation>(proj).Value;
            //entityManager.SetComponentData<Rotation>(proj, new Rotation { Value = quaternion.AxisAngle(new float3(0,0,1), rotation.Value.value.y) });

            // print("Rotation: x." + math.radians(rotation.Value.value.x) + " y." + math.radians(rotation.Value.value.y) + " z." + math.radians(rotation.Value.value.z) + " w." + math.radians(rotation.Value.value.w));

            // entityManager.SetComponentData<Rotation>(proj, new Rotation { Value = new quaternion(currRot.Value.value.x,rotation.Value.value.y, currRot.Value.value.z, currRot.Value.value.w) });
            entityManager.SetComponentData<MoveForwardRuntime>(proj, new MoveForwardRuntime { speedDivisor = playerComp.speedDivisor, damage = playerComp.damage });
            entityManager.SetComponentData<Rotation>(proj, new Rotation { Value = rotation.Value });

        }
    }
}
