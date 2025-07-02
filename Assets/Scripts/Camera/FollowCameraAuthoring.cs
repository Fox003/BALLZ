using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class FollowCameraAuthoring : MonoBehaviour
{
    public float FollowSpeed;
    public float RotationSpeed;
    public float ZoomScale;
    public float MinZoom;
    public float MaxZoom;
    public float3 Offset;
}

class FollowCameraAuthoringBaker : Baker<FollowCameraAuthoring>
{
    public override void Bake(FollowCameraAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        
        AddComponent(entity, new FollowCameraData
        {
            FollowSpeed = authoring.FollowSpeed,
            ZoomScale = authoring.ZoomScale,
            MinZoom = authoring.MinZoom,
            MaxZoom = authoring.MaxZoom,
            Offset =  authoring.Offset,
            RotationSpeed = authoring.RotationSpeed,
        });
        
        AddComponent<CameraTag>(entity);
    }
}
