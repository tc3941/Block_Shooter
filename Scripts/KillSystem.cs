using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;
using UnityEngine;

public class KillSystem : JobComponentSystem
{
    // EndFrameBarrier provides the CommandBuffer
    EntityManager m_EndFrameBarrier;

    
    struct SpawnJob : IJobForEachWithEntity<AABB>
    {
        //public EntityManager CommandBuffer;
        public void Execute(Entity entity, int index, [ReadOnly] ref AABB ab)
        {
         //   if(ab.dead)
         //   CommandBuffer.DestroyEntity(entity);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        // Schedule the job that will add Instantiate commands to the EntityCommandBuffer.

        var job = new SpawnJob
        {
          //  CommandBuffer = World.Active.EntityManager
        }.ScheduleSingle(this, inputDeps);

        // We need to tell the barrier system which job it needs to complete before it can play back the commands.

        return job;
    }
}