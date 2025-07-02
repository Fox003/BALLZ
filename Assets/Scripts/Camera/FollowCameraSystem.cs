using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct FollowCameraSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        bool firstTarget = true;
        float3 minPos = float3.zero;
        float3 maxPos = float3.zero;
        
        foreach (var transform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<CameraTargetTag>())
        {
            if (firstTarget)
            {
                minPos = maxPos = transform.ValueRO.Position;
                firstTarget = false;
            }
            else
            {
                minPos = math.min(minPos, transform.ValueRO.Position);
                maxPos = math.max(maxPos, transform.ValueRO.Position);
            }
        }
        
        // Calculate the middle point of all those positions
        float3 center = (minPos + maxPos) * 0.5f;
        float spread = math.distance(minPos, maxPos);

        foreach (var (settings, transform) in SystemAPI.Query<RefRO<FollowCameraData>, RefRW<LocalTransform>>())
        {
            // Scale factor: tweak this to get the right zoom feel
            float zoomMultiplier = math.clamp(spread * settings.ValueRO.ZoomScale, settings.ValueRO.MinZoom, settings.ValueRO.MaxZoom);

            // Scale the offset
            float3 scaledOffset = settings.ValueRO.Offset * zoomMultiplier;
            
            float3 desiredPos = center + scaledOffset;
            
            transform.ValueRW.Position = math.lerp(
                transform.ValueRO.Position,
                desiredPos,
                settings.ValueRO.FollowSpeed * SystemAPI.Time.DeltaTime
            );
            
            float3 direction = math.normalize(center - transform.ValueRW.Position);
            quaternion lookRotation = quaternion.LookRotationSafe(direction, math.up());

            transform.ValueRW.Rotation = math.slerp(
                transform.ValueRW.Rotation,
                lookRotation,
                settings.ValueRO.RotationSpeed * SystemAPI.Time.DeltaTime
            );
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
