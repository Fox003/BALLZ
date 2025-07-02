using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct PlayerMovementData : IComponentData
{
    public float MaxSpeed;
    public float Acceleration;
    public float MaxAccelerationForce;
    public float2 ForceScale;
}

public struct PlayerVelocityData : IComponentData
{
    public float2 GoalVelocity;
}