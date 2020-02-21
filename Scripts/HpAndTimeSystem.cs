﻿using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;
using UnityEngine;

public class HpAndTimeSystem : JobComponentSystem
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
    struct HpAndTimeSystemJob : IJobForEach<Translation, Rotation, HpAndTimeRunTime>
    {
        // Add fields here that your job needs to do its work.
        // For example,
        //    public float deltaTime;

        public bool timeStopped, paused,toggled,toggledWorked, outOfMp;
        public float deltaTime;
        
        
        public void Execute(ref Translation translation, [ReadOnly] ref Rotation rotation, ref HpAndTimeRunTime data)
        {
            toggledWorked = false;
            outOfMp = false;
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
                if (toggled && (data.mpCurrent > data.mpMin))
                {
                    if (!data.timeStopped)
                        data.mpCurrent -= 10;
                    //toggledWorked = true;
                    data.timeStopped = !data.timeStopped;
                }

                if (toggled && (data.mpCurrent < data.mpMin))
                {
                    data.timeStopped = !data.timeStopped;
                }

                if (data.hpCurrent + (data.hps * deltaTime) > data.hpMax)
                    data.hpCurrent = data.hpMax;
                else
                    data.hpCurrent += data.hps * deltaTime;

                if (data.mpCurrent + (data.mps * deltaTime) > data.mpMax)
                    data.mpCurrent = data.mpMax;
                else if (data.timeStopped)
                    data.mpCurrent -= data.mps * deltaTime;
                else
                    data.mpCurrent += data.mps * deltaTime;

                if (data.mpCurrent <= 0)
                {
                    data.mpCurrent = 0;
                    if (data.timeStopped)
                        data.timeStopped = !data.timeStopped;

                }
            }
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var toggled = Input.GetButtonDown("Jump");
        bool dead = false;
        if (Camera.main.GetComponent<HpAndTimeScale>() != null)
            dead = Camera.main.GetComponent<HpAndTimeScale>().GetDead();


        var job = new HpAndTimeSystemJob
        {
            toggled = toggled,
            //timeStopped = GameWorldVars.TimeStopped,
            paused = PauseSystem.Paused|| dead,
            deltaTime = Time.deltaTime
        };
       
        // Assign values to the fields on your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        //     job.deltaTime = UnityEngine.Time.deltaTime;
        //job.deltaTime = UnityEngine.Time.deltaTime;
        
        
        
        // Now that the job is set up, schedule it to be run. 
        return job.Schedule(this, inputDependencies);
    }
}