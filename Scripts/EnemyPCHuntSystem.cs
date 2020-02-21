using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;

public class EnemyPCHuntSystem : JobComponentSystem
{
    Translation PC;
    // This declares a new kind of job, which is a unit of work to do.
    // The job is declared as an IJobForEach<Translation, Rotation>,
    // meaning it will process all entities in the world that have both
    // Translation and Rotation components. Change it to process the component
    // types you want.
    //
    // The job is also tagged with the BurstCompile attribute, which means
    // that the Burst compiler will optimize it for the best performance.
    [BurstCompile]
    struct EnemyPCHuntSystemJob : IJobForEach<Translation, Rotation, EnemyPCHuntRuntime>
    {
        // Add fields here that your job needs to do its work.
        // For example,
        //    public float deltaTime;
        
        public Translation playerLoc;
        public bool freeze,paused;
        public void Execute(ref Translation translation, [ReadOnly] ref Rotation rotation, ref EnemyPCHuntRuntime data)
        {
            // Implement the work to perform for each entity here.
            // You should only access data that is local or that is a
            // field on this job. Note that the 'rotation' parameter is
            // marked as [ReadOnly], which means it cannot be modified,
            // but allows this job to run in parallel with other jobs
            // that want to read Rotation component data.
            // For example,
            //     translation.Value += mul(rotation.Value, new float3(0, 0, 1)) * deltaTime;
            if (!paused)
            {
                
                float timeMulti = 1;
                if (freeze && !data.timeEnemy)
                    timeMulti = 0;

                var enemyLoc = translation.Value;
                var pos = new float3(enemyLoc.x, enemyLoc.y, enemyLoc.z);
                var PCpos = new float3(playerLoc.Value.x, playerLoc.Value.y, playerLoc.Value.z);

                var dif = math.normalize(PCpos - pos);
                translation.Value += (dif / data.speedDivisor) * timeMulti;
            }
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        if(Camera.main.GetComponent<PlayerSpawn>()!=null)
        PC = Camera.main.GetComponent<PlayerSpawn>().GetPcPosition();

        HpAndTimeRunTime PCTime = new HpAndTimeRunTime();
        if (Camera.main.GetComponent<PlayerSpawn>() != null&& Camera.main.GetComponent<PlayerSpawn>().PC!=null)
        PCTime = World.Active.EntityManager.GetComponentData<HpAndTimeRunTime>(Camera.main.GetComponent<PlayerSpawn>().PC);

        bool dead = false;
        if (Camera.main.GetComponent<HpAndTimeScale>() != null)
            dead = Camera.main.GetComponent<HpAndTimeScale>().GetDead();

        var job = new EnemyPCHuntSystemJob()
        {
            freeze = PCTime.timeStopped,
            playerLoc = PC,
            paused = PauseSystem.Paused || dead,
        };
        
        // Assign values to the fields on your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        //     job.deltaTime = UnityEngine.Time.deltaTime;
        
        
        
        // Now that the job is set up, schedule it to be run. 
        return job.Schedule(this, inputDependencies);
    }
}