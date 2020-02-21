using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;


public static class CollisionLogic
{

    public static bool Intersect(AABB box1, AABB box2)
    {
        return (box1.min.x <= box2.max.x && box1.max.x >= box2.min.x) &&
               (box1.min.y <= box2.max.y && box1.max.y >= box2.min.y) &&
               (box1.min.z <= box2.max.z && box1.max.z >= box2.min.z);
    }
    public static void IntersectDestory(AABB box1, AABB box2)
    {
        if (Intersect(box1, box2))
        {
            var entityManager = World.Active.EntityManager;
            entityManager.DestroyEntity(box1.self);
            entityManager.DestroyEntity(box2.self);
        }
    }
}
