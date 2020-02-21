using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class GameWorldVars : MonoBehaviour
{
    static bool timeStopped = false;
    static bool paused = false;
    public Entity player, projectile;
    EntityManager entityManager;
    public static float shotDelay, shotReduction;
    public static float shotDelayMin;

    // Start is called before the first frame update
    public static bool Paused
    {
        get
        {
            return paused;
        }
    }

    public static bool TimeStopped
    {
        get
        {
            return timeStopped;
        }
    }
    void Start()
    {
        entityManager = World.Active.EntityManager;
        shotDelay = .5f;
        shotReduction = .01f;
        shotDelayMin = .1f;
    }

    // Update is called once per frame
    void Update()
    {//check for time vars here
        
    }

    public static void decreaseShotDelay()
    {
        if (shotDelay >= shotDelayMin)
            shotDelay -= shotReduction;

        print("New Shot Delay: " + shotDelay);
    }
    
    public static void toggleTimeStop()
    {
        timeStopped = !timeStopped;
    }
    public static void togglePaused()
    {
        paused = !paused;
    }
    public Entity GetProjectile()
    {
        return projectile;
    }
}
