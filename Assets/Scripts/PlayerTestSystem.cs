using Unity.Burst;
using Unity.Entities;
using UnityEngine;

partial struct PlayerTestSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var playerAspect in SystemAPI.Query<PlayerAspect>())
        {
            var playerInputData = playerAspect.PlayerInputData;
            
            if (playerInputData.ValueRO.Jump)
                Debug.Log("Jump was true!");
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
