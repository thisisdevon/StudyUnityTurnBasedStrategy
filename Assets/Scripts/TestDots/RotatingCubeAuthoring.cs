using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class RotatingCubeAuthoring : MonoBehaviour
{
    public class Baker : Baker<RotatingCubeAuthoring>
    {
        public override void Bake(RotatingCubeAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new RotatingCube());
        }
    }
}

public struct RotatingCube : IComponentData
{
    //if empty without any properties itd be registered as tag instead
}