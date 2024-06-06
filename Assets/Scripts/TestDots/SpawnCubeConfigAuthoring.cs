using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SpawnCubeConfigAuthoring : MonoBehaviour
{
    public GameObject cubePrefab;
    public int spawnAmount;

    public class Baker : Baker<SpawnCubeConfigAuthoring>
    {
        public override void Bake(SpawnCubeConfigAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new SpawnCubesConfig
            {
                cubePrefabEntity = GetEntity(authoring.cubePrefab, TransformUsageFlags.Dynamic),
                spawnAmount = authoring.spawnAmount,
            });
        }
    }
}

public struct SpawnCubesConfig : IComponentData
{
    public Entity cubePrefabEntity;
    public int spawnAmount;
}
