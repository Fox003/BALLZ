using Inputs;
using Unity.Entities;
using UnityEngine;

class PlayerAuthoring : MonoBehaviour
{
    
}

class PlayerAuthoringBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        
        AddComponent<PlayerTag>(entity);
        AddComponent<PlayerInputData>(entity);
        AddComponent<PlayerInputDevice>(entity);
        AddComponent<CameraTargetTag>(entity);
    }
}

public struct PlayerTag : IComponentData {}
