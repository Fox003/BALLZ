using Unity.Entities;
using Unity.Mathematics;

public struct CameraTargetTag : IComponentData {}
public struct CameraTag : IComponentData {}

public struct FollowCameraData : IComponentData
{
    public float3 Offset;
    public float FollowSpeed;
    public float RotationSpeed;
    public float MinZoom;
    public float MaxZoom;
    public float ZoomScale;
}
