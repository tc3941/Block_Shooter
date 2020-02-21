using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct AABB : IComponentData
{
    // Add fields to your component here. Remember that:
    //
    // * A component itself is for storing data and doesn't 'do' anything.
    //
    // * To act on the data, you will need a System.
    //
    // * Data in a component must be blittable, which means a component can
    //   only contain fields which are primitive types or other blittable
    //   structs; they cannot contain references to classes.
    //
    // * You should focus on the data structure that makes the most sense
    //   for runtime use here. Authoring Components will be used for 
    //   authoring the data in the Editor.
    public float3 min;
    public float3 max;
    public Entity self;
    public int id;
    public bool hit;
    public float spawnDelay, maxLifetime;
    //Player = 0, enemy = 1, bullet = 2


}
