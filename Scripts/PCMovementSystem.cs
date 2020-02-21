﻿using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

using static Unity.Mathematics.math;

public class PCMovementSystem : JobComponentSystem
{
    // This declares a new kind of job, which is a unit of work to do.
    // The job is declared as an IJobForEach<Translation, Rotation>,
    // meaning it will process all entities in the world that have both
    // Translation and Rotation components. Change it to process the component
    // types you want.
    //
    // The job is also tagged with the BurstCompile attribute, which means
    // that the Burst compiler will optimize it for the best performance.
    [BurstCompile]
    struct PCMovementSystemJob : IJobForEach<Translation, Rotation, PCMovementRunTime>
    {
        // Add fields here that your job needs to do its work.
        // For example,
        //    public float deltaTime;

        public float moveX,moveZ,deltaTime;
        public bool paused;

        public void Execute(ref Translation translation, [ReadOnly] ref Rotation rotation, ref PCMovementRunTime moveData)
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
                moveData.playerX = moveX;
                moveData.playerZ = moveZ;
                translation.Value += (new float3(moveData.playerX, 0, moveData.playerZ) / moveData.speedDivisor);
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        bool dead = false;
        if (Camera.main.GetComponent<HpAndTimeScale>()!=null)
        dead = Camera.main.GetComponent<HpAndTimeScale>().GetDead();

        var job = new PCMovementSystemJob {
            moveX = Input.GetAxis("Horizontal"),
            moveZ = Input.GetAxis("Vertical"),
            deltaTime = Time.deltaTime,
            paused = PauseSystem.Paused || dead
        };
        //job.moveData.playerX = Input.GetAxis("Horizontal"); moveData.playerZ = Input.GetAxis("Vertical");
        // Assign values to the fields on your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        //     job.deltaTime = UnityEngine.Time.deltaTime;d



        // Now that the job is set up, schedule it to be run. 
        return job.Schedule(this, inputDependencies);
    }
}