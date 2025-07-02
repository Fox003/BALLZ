using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using UnityEngine;

partial struct PlayerCollisionsSystem : ISystem
{
    private ComponentLookup<PlayerTag> _playerLookup;
    private ComponentLookup<PhysicsMass> _physicsMassLookup;
    private ComponentLookup<PhysicsVelocity> _physicsVelocityLookup;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _playerLookup = state.GetComponentLookup<PlayerTag>(true);
        _physicsMassLookup = state.GetComponentLookup<PhysicsMass>(false);
        _physicsVelocityLookup = state.GetComponentLookup<PhysicsVelocity>(false);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        _playerLookup.Update(ref state);
        _physicsMassLookup.Update(ref state);
        _physicsVelocityLookup.Update(ref state);

        state.Dependency = new PlayerCollisionsJob()
        {
            PlayerLookup = _playerLookup,
            PhysicsMassLookup = _physicsMassLookup,
            PhysicsVelocityLookup = _physicsVelocityLookup,
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

public partial struct PlayerCollisionsJob : ICollisionEventsJob
{
    [ReadOnly] public ComponentLookup<PlayerTag> PlayerLookup;
    public ComponentLookup<PhysicsMass> PhysicsMassLookup;
    public ComponentLookup<PhysicsVelocity> PhysicsVelocityLookup;
    
    
    public void Execute(CollisionEvent collisionEvent)
    {
        var entityA = collisionEvent.EntityA;
        var entityB = collisionEvent.EntityB;

        bool aIsPlayer = PlayerLookup.HasComponent(entityA);
        bool bIsPlayer = PlayerLookup.HasComponent(entityB);

        // Only proceed if A is player and B is not (or some direction you define)
        if (aIsPlayer && !bIsPlayer)
        {
            // Handle collision where player hit something else
        }
        else if (aIsPlayer && bIsPlayer && entityA.Index < entityB.Index)
        {
            Debug.Log("Players touched...");
            var mass =  PhysicsMassLookup[entityA];
            var velocity =  PhysicsVelocityLookup[entityA];
            velocity.ApplyLinearImpulse(mass, new float3(0, 5, 0));
            PhysicsVelocityLookup[entityA] = velocity;
        }
    }
}
