using Inputs;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
partial struct PlayerInputSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }
    
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (inputDevice, inputData) in SystemAPI.Query<RefRO<PlayerInputDevice>, RefRW<PlayerInputData>>())
        {
            int deviceID = inputDevice.ValueRO.DeviceID;

            if (!PlayerInputRegistry.TryGetPlayerInput(deviceID, out var playerInput))
            {
                Debug.Log("Player input not found");
                return;
            }

            if (playerInput == null)
            {
                Debug.Log("Player input was null");
                return;
            }

            inputData.ValueRW.Jump = playerInput.actions.FindAction("Jump", true).triggered;
            inputData.ValueRW.Move = (float2)playerInput.actions.FindAction("Move", true).ReadValue<Vector2>();
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
