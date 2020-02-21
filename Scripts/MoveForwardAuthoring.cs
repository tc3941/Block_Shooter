using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Transforms;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class MoveForwardAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    // Add fields to your component here. Remember that:
    //
    // * The purpose of this class is to store data for authoring purposes - it is not for use while the game is
    //   running.
    // 
    // * Traditional Unity serialization rules apply: fields must be public or marked with [SerializeField], and
    //   must be one of the supported types.
    //
    // For example,
    //    public float scale;

    public float speedDivisor,damage, maxTime;



    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        // Call methods on 'dstManager' to create runtime components on 'entity' here. Remember that:
        //
        // * You can add more than one component to the entity. It's also OK to not add any at all.
        //
        // * If you want to create more than one entity from the data in this class, use the 'conversionSystem'
        //   to do it, instead of adding entities through 'dstManager' directly.
        //
        // For example,
        //   dstManager.AddComponentData(entity, new Unity.Transforms.Scale { Value = scale });
        var loc = dstManager.GetComponentData<Translation>(entity);
        var aabb = new AABB
        {
            max = loc.Value + .3f,
            min = loc.Value - .3f,
            self = entity,
            id = 2,
            hit = false,
            maxLifetime = maxTime
        };
        dstManager.AddComponentData(entity, new MoveForwardRuntime { speedDivisor = speedDivisor, damage = damage, maxTime = maxTime});
        dstManager.AddComponentData(entity,  aabb);
    }
}
