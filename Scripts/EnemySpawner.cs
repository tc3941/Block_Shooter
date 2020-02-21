using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class EnemySpawner : MonoBehaviour
{
    public GameObject projectilePlayer;
    public float2 rngRange, timeEnemySpawnRange;
    [ReadOnly]public static float spawnInterval;
    const float spawnRange = 50;
    const float numOfWalls = 4;
    static float minInterval;
    public GameObject EnemyPrefab,SecondEnemyPrefab;
    Entity Enemy, SecondEnemy,projectile;
    public GameObject[] PlayerWallObjects = new GameObject[4];
    Entity[] PlayerWallEntities = new Entity[4];
    EntityManager entityManager;
    // Start is called before the first frame update
    void Start()
    {
       
       // print("Starting Spawner.");
        StartCoroutine(SpawnEnemy());
        Enemy = GameObjectConversionUtility.ConvertGameObjectHierarchy(EnemyPrefab, World.Active);
        SecondEnemy = GameObjectConversionUtility.ConvertGameObjectHierarchy(SecondEnemyPrefab, World.Active);
        projectile = GameObjectConversionUtility.ConvertGameObjectHierarchy(projectilePlayer, World.Active);
        entityManager = World.Active.EntityManager;
        Camera.main.GetComponent<GameWorldVars>().projectile = projectile;
        GetSpawnWalls();
        spawnInterval = 1;
        minInterval = .1f;
    }

    void GetSpawnWalls()
    {
        for(int i = 0; i <= PlayerWallObjects.Length - 1; i++)
        {
            PlayerWallEntities[i] = GameObjectConversionUtility.ConvertGameObjectHierarchy(PlayerWallObjects[i], World.Active);
            entityManager.AddComponentData<Scale>(PlayerWallEntities[i], new Scale { Value = PlayerWallObjects[i].transform.localScale });
        }
        /*
        PlayerWallEntities[0] = GameObjectConversionUtility.ConvertGameObjectHierarchy(PlayerWallObjects[0], World.Active);
        PlayerWallEntities[1] = GameObjectConversionUtility.ConvertGameObjectHierarchy(PlayerWallObjects[1], World.Active);
        PlayerWallEntities[2] = GameObjectConversionUtility.ConvertGameObjectHierarchy(PlayerWallObjects[2], World.Active);
        PlayerWallEntities[3] = GameObjectConversionUtility.ConvertGameObjectHierarchy(PlayerWallObjects[3], World.Active);

        entityManager.AddComponentData<Scale>(PlayerWallEntities[0], new Scale { Value = PlayerWallObjects[0].transform.localScale });
        */
    }

    public IEnumerator SpawnEnemy()
    {
        print("Inside");
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (!PauseSystem.Paused && Camera.main.GetComponent<HpAndTimeScale>().GetAlive())
                SelectSpawnLocationAndSpawn();
        }
    }

    void SelectSpawnLocationAndSpawn()
    {
        
        var rng = UnityEngine.Random.Range(rngRange.x, rngRange.y);
        // print(rng);
        Entity enemy;
        bool timeEnemy = rng <= timeEnemySpawnRange.y && rng >= timeEnemySpawnRange.x;
        if (timeEnemy)
            enemy = entityManager.Instantiate(SecondEnemy);
        else
            enemy = entityManager.Instantiate(Enemy);

        var x = UnityEngine.Random.Range(-spawnRange, spawnRange);
        var z = UnityEngine.Random.Range(-spawnRange, spawnRange);
        var wall = UnityEngine.Random.Range(0, numOfWalls);
        var spawnBlockLoc = entityManager.GetComponentData<Translation>(PlayerWallEntities[(int)wall]);
        var spawnBlockScale = entityManager.GetComponentData<Scale>(PlayerWallEntities[(int)wall]);

        entityManager.SetComponentData(enemy, new Translation {
            Value = new float3(spawnBlockLoc.Value.x+(spawnBlockScale.Value.x*x),1,spawnBlockLoc.Value.z+ (spawnBlockScale.Value.z * z))
        });;
        /*
        print("Enemy Spawn Tick. Current Time: " + Time.time + ". Current Interval: " + spawnInterval + 
            " Pos: " + x + ", " + z + 
            ", Side: " + (int)wall);*/

    }

    public struct Scale : IComponentData
    {
        public float3 Value;
    }

    public static void decreaseSpawnInterval(float time)
    {
        if(spawnInterval>= minInterval)
        spawnInterval -= time;

        print("New Spawn interval: " + spawnInterval);
        HpAndTimeScale.incrementScore();
        GameWorldVars.decreaseShotDelay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
public struct EnemySpawnerStruct
{
    public static void decreaseSpawnInterval(float time)
    {
        EnemySpawner.decreaseSpawnInterval(time);
    }

}
