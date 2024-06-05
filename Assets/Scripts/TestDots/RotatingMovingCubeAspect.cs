using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct RotatingMovingCubeAspect : IAspect
{
    // so that only those with RotatingCube component be selected,
    // alternatively you can remove it if you already using WithAll
    // public readonly RefRO<RotatingCube> rotatingCube; 
    
    public readonly RefRW<LocalTransform> localTransform;
    public readonly RefRO<RotateSpeed> rotateSpeed;
    public readonly RefRO<Movement> movement;

    public void MoveAndRotate(float deltaTime)
    {
        localTransform.ValueRW =
            localTransform.ValueRO.RotateY(rotateSpeed.ValueRO.value * deltaTime);
            
        localTransform.ValueRW =
            localTransform.ValueRO.Translate(movement.ValueRO.movementVector * deltaTime);
    }
}
