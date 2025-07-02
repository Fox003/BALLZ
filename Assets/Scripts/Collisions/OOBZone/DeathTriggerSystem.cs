using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public struct OOBZone : IComponentData
{
    public OOBZoneType OOBZoneType;
    public float3 RespawnPosition;
}

public enum OOBZoneType
{
    RESPAWN = 0,
    KILL = 1
}

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(EndFixedStepSimulationEntityCommandBufferSystem))]
public partial struct DeathTriggerSystemGroup : ISystem
{
    private ComponentLookup<PlayerTag> _playerLookup;
    private ComponentLookup<OOBZone> _deathZoneLookup;
    private ComponentLookup<LocalTransform> _localTransformLookup;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _playerLookup = state.GetComponentLookup<PlayerTag>(true);
        _deathZoneLookup = state.GetComponentLookup<OOBZone>(true);
        _localTransformLookup = state.GetComponentLookup<LocalTransform>(false);
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSystem = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);

        _playerLookup.Update(ref state);
        _deathZoneLookup.Update(ref state);
        _localTransformLookup.Update(ref state);

        var job = new DeathTriggerSystem
        {
            PlayerLookup = _playerLookup,
            OOBZoneLookup = _deathZoneLookup,
            LocalTransformLookup = _localTransformLookup,
            Ecb = ecb
        };

        var sim = SystemAPI.GetSingleton<SimulationSingleton>();
        state.Dependency = job.Schedule(sim, state.Dependency);
    }
}

public partial struct DeathTriggerSystem : ITriggerEventsJob
{
    [ReadOnly] public ComponentLookup<PlayerTag> PlayerLookup;
    [ReadOnly] public ComponentLookup<OOBZone> OOBZoneLookup;
    public ComponentLookup<LocalTransform> LocalTransformLookup;
    public EntityCommandBuffer Ecb;
    
    public void Execute(TriggerEvent triggerEvent)
    {
        var entityA = triggerEvent.EntityA;
        var entityB = triggerEvent.EntityB;

        bool aIsPlayer = PlayerLookup.HasComponent(entityA);
        bool bIsPlayer = PlayerLookup.HasComponent(entityB);

        bool aIsDeathZone = OOBZoneLookup.HasComponent(entityA);
        bool bIsDeathZone = OOBZoneLookup.HasComponent(entityB);
        
        if (aIsPlayer && bIsDeathZone)
        {
            if (OOBZoneLookup[entityB].OOBZoneType == OOBZoneType.KILL)
            {
                Ecb.DestroyEntity(entityA);
            }
            else
            {
                var newTransform = LocalTransform.FromPosition(OOBZoneLookup[entityB].RespawnPosition);
                Ecb.SetComponent(entityA, newTransform);
            }
        }
        else if (bIsPlayer && aIsDeathZone)
        {
            if (OOBZoneLookup[entityA].OOBZoneType == OOBZoneType.KILL)
            {
                Ecb.DestroyEntity(entityB);
            }
            else
            {
                var newTransform = LocalTransform.FromPosition(OOBZoneLookup[entityA].RespawnPosition);
                Ecb.SetComponent(entityB, newTransform);
            }
        }
    }
}
