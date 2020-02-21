using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;

public class AABBSystem : JobComponentSystem
{
    // This declares a new kind of job, which is a unit of work to do.
    // The job is declared as an IJobForEach<Translation, Rotation>,
    // meaning it will process all entities in the world that have both
    // Translation and Rotation components. Change it to process the component
    // types you want.
    //
    // The job is also tagged with the BurstCompile attribute, which means
    // that the Burst compiler will optimize it for the best performance.
    public EntityCommandBuffer ecb;
    static public NativeList<AABB> aabbArray;
    EntityCommandBufferSystem bufferSystem;
    EndSimulationEntityCommandBufferSystem m_EntityCommandBufferSystem;


    
    struct AABBSystemJob : IJobParallelFor
    {
        // Add fields here that your job needs to do its work.
        // For example,
        //    public float deltaTime;
        [NativeDisableParallelForRestriction] public NativeArray<AABB> aabbArray;
        //[ReadOnly]EntityCommandBuffer ECB;
        //public EntityManager em;
        [ReadOnly] public EntityCommandBuffer buffer2;
        [ReadOnly] public EntityCommandBuffer.Concurrent buffer;
        [ReadOnly] public Entity PC;



        public void Execute(int i)
        {
            // Implement the work to perform for each entity here.
            // You should only access data that is local or that is a
            // field on this job. Note that the 'rotation' parameter is
            // marked as [ReadOnly], which means it cannot be modified,
            // but allows this job to run in parallel with other jobs
            // that want to read Rotation component data.
            // For example,
            //     translation.Value += mul(rotation.Value, new float3(0, 0, 1)) * deltaTime;
            //var array = aabbArray[i];

            for (int j = i + 1; j < aabbArray.Length; j++)
                {
                    if (CollisionLogic.Intersect(aabbArray[i], aabbArray[j])&& aabbArray[j].self!= aabbArray[i].self&&aabbArray[i].id!= aabbArray[j].id)
                    {
                    //Debug.Log("Collision Detected " + aabbArray[i].id + " vs " + aabbArray[j].id);
                    // Debug.Log("Collision Detected " + aabbArray[i].self + " vs " + aabbArray[j].self);
                    var hitPlayer = false;
                    if ((aabbArray[i].id == 0&& aabbArray[j].id == 1))
                    {
                        //  var pc = aabbArray[i];
                        //  pc.hit = true;
                       // buffer.SetComponent<AABB>(i, PC, new AABB { hit = true, id = 0, self = PC });

                        //buffer.RemoveComponent<PCHitRunTime>(i, PC);
                        //buffer.AddComponent<PCHitRunTime>(i, PC, new PCHitRunTime { hit = true });
                        buffer.DestroyEntity(i, aabbArray[j].self);
                        HpAndTimeScale.HitPlayer();
                        hitPlayer = true;
                        // aabbArray[i].id = 2;
                        // em.GetComponentData<PCHitRunTime>(PC);
                    }
                    if ((aabbArray[i].id == 1 && aabbArray[j].id == 0))
                    {
                        //var pc = aabbArray[i];
                       // pc.hit = true;
                       // buffer.SetComponent<AABB>(i, PC, new AABB { hit = true, id = 0,self = PC });
                        //var pc = aabbArray[j];
                        //pc.hit = true;
                        //buffer.SetComponent<PCHitRunTime>(i, PC, new PCHitRunTime { hit = true });

                        buffer.DestroyEntity(i, aabbArray[i].self);
                        HpAndTimeScale.HitPlayer();
                        hitPlayer = true;
                        //em.GetComponentData<PCHitRunTime>(PC);
                    }
                    if (aabbArray[i].id == 1 || aabbArray[i].id == 2)
                    {
                        if (aabbArray[i].id == 1&&hitPlayer == false)
                            EnemySpawner.decreaseSpawnInterval(aabbArray[i].spawnDelay);
                        buffer.DestroyEntity(i, aabbArray[i].self);
                    }

                    if (aabbArray[j].id == 1 || aabbArray[j].id == 2)
                    {
                        if(aabbArray[j].id == 1 && hitPlayer == false)
                        EnemySpawner.decreaseSpawnInterval(aabbArray[j].spawnDelay);

                        buffer.DestroyEntity(i, aabbArray[j].self);
                    }

                    break;
                    }
                }
            if (aabbArray[i].maxLifetime <= 0&&aabbArray[i].id==2)
                buffer.DestroyEntity(i, aabbArray[i].self);
            
        }
    }

    EntityQuery entityQuery;

    protected override void OnCreate()
    {
        var query = new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(AABB) }
        };

        entityQuery = GetEntityQuery(query);

        m_EntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        // ecb = new ComponentSystem().PostUpdateCommands;
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var colliders = entityQuery.ToComponentDataArray<AABB>(Allocator.Persistent);

        // bufferSystem =
        //bufferSystem = World.CreateSystem<EntityCommandBufferSystem>();
        var commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();
        var commandBuffer2 = m_EntityCommandBufferSystem.CreateCommandBuffer();
        var job = new AABBSystemJob
        {
            aabbArray = colliders,
            buffer = commandBuffer,
           PC = Camera.main.GetComponent<PlayerSpawn>().PC,
            buffer2 = commandBuffer2
        };
        var compJob = job.Schedule(colliders.Length,100);
        compJob.Complete();


        

        //foreach (AABB ab in aabbArray)
        //    commandBuffer.DestroyEntity(ab.self);

        //aabbArray.Clear();

        // Assign values to the fields on your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        //     job.deltaTime = UnityEngine.Time.deltaTime;

        colliders.Dispose();

        // Now that the job is set up, schedule it to be run. 
        return compJob;
    }
}