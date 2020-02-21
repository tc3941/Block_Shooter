using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class collisionsTest : MonoBehaviour
{
    PhysicsShapeAuthoring shapeAuthoring;
    
        // Start is called before the first frame update
    void Start()
    {
        shapeAuthoring = GetComponent<PhysicsShapeAuthoring>();
    }

    // Update is called once per frame
    void Update()
    {
        var entity = Camera.main.GetComponent<GameWorldVars>().projectile;
        var EM = World.Active.EntityManager;
        print("Shape: " + shapeAuthoring.ShapeType);

    }
    
}
