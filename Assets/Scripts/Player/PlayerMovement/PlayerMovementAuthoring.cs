using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class PlayerMovementAuthoring : MonoBehaviour
{
    public float MaxSpeed;
    public float Acceleration;
    public float MaxAccelerationForce;
}

class PlayerMovementAuthoringBaker : Baker<PlayerMovementAuthoring>
{
    public override void Bake(PlayerMovementAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new PlayerMovementData
        {
            MaxSpeed = authoring.MaxSpeed,
            Acceleration = authoring.Acceleration,
            MaxAccelerationForce = authoring.MaxAccelerationForce
        });
        
        AddComponent(entity, new PlayerVelocityData
        {
            GoalVelocity = new float2(0, 0)
        });
    }
}
