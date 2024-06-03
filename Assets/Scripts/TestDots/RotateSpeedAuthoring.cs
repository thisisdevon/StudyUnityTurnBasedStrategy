using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class RotateSpeedAuthoring : MonoBehaviour
{
    public float value;

    private class Baker : Baker<RotateSpeedAuthoring>
    {
        public override void Bake(RotateSpeedAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new RotateSpeed
            {
                //Register the value to the entity
                value = authoring.value
            });
        }
    }
}

public partial struct RotateSpeed : IComponentData
{
    //Component should only consists of data without function
    public float value;
}
