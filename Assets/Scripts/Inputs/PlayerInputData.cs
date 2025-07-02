using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public struct PlayerInputData : IComponentData
    {
        public bool Jump;
        public bool Dash;
        public float2 Move;
    }
    
    public struct PlayerInputDevice : IComponentData
    {
        public int DeviceID;
    }
}
