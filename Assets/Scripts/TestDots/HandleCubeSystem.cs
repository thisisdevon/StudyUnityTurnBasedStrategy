using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct HandleCubeSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        MoveAndRotateJob moveAndRotateJob = new MoveAndRotateJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        };
        moveAndRotateJob.ScheduleParallel();
    }
    
    
    [BurstCompile]
    [WithAll(typeof(RotatingCube))]
    public partial struct MoveAndRotateJob : IJobEntity
    {
        public float deltaTime;
        public void Execute(RotatingMovingCubeAspect rotatingMovingCubeAspect)
        {
            rotatingMovingCubeAspect.MoveAndRotate(deltaTime);
        }
    }
}
