using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
partial struct CameraEntityBootstrap : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CameraTag>();
    }


    public void OnUpdate(ref SystemState state)
    {
        var cameraEntity = SystemAPI.GetSingletonEntity<CameraTag>();

        var behaviour = Object.FindFirstObjectByType<ECSCameraFollower>();
        if (behaviour != null && behaviour.CameraEntity == Entity.Null)
        {
            behaviour.CameraEntity = cameraEntity;
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
