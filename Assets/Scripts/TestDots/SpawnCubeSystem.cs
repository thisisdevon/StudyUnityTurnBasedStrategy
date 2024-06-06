using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class SpawnCubeSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<SpawnCubesConfig>();
    }

    protected override void OnUpdate()
    {
        this.Enabled = false;
        
        // if none found return error
        // if more than one found return error too
        SpawnCubesConfig spawnCubesConfig = SystemAPI.GetSingleton<SpawnCubesConfig>();
        for (int i = 0; i < spawnCubesConfig.spawnAmount; i++)
        {
            Debug.Log("Spawning cube: " + i);
            Entity spawnCube = EntityManager.Instantiate(spawnCubesConfig.cubePrefabEntity);
            EntityManager.SetComponentData(spawnCube,
                LocalTransform.FromPosition(new Unity.Mathematics.float3(
                    UnityEngine.Random.Range(-10f, 5f), 
                    0.6f,
                    UnityEngine.Random.Range(-4f, 7f)
                    )
                )
            );
        }
    }
}
