using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct RotatingCubeSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<RotateSpeed>();
    }
    
    [BurstCompile]
    void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<LocalTransform> localTransform, RefRO<RotateSpeed> rotateSpeed) 
            in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateSpeed>>())
        {
            float power = 1f;
            for (int i = 0; i < 10000; i++)
            {
                power *= 2f;
                power /= 2f;
            }
            localTransform.ValueRW = localTransform.ValueRO.RotateY(rotateSpeed.ValueRO.value * SystemAPI.Time.DeltaTime * power);
        }
    }
}
