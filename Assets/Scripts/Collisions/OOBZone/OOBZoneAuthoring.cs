using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class OOBZoneAuthoring : MonoBehaviour
{
    public OOBZoneType ZoneType;
    public Transform RespawnPoint;
}

class OOBZoneAuthoringBaker : Baker<OOBZoneAuthoring>
{
    public override void Bake(OOBZoneAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);

        if (authoring.RespawnPoint == null)
        {
            AddComponent(entity, new OOBZone
            {
                OOBZoneType = authoring.ZoneType,
                RespawnPosition = float3.zero,
            });
        }
        else
        {
            AddComponent(entity, new OOBZone
            {
                OOBZoneType = authoring.ZoneType,
                RespawnPosition = authoring.RespawnPoint.position,
            });
        }
    }
}
