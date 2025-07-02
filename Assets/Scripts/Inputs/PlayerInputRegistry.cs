using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Inputs
{
    public static class PlayerInputRegistry
    {
        private static Dictionary<int, PlayerInput> PlayerInputs = new();

        public static void AddPlayerInput(PlayerInput playerInput)
        {
            PlayerInputs.Add(playerInput.user.pairedDevices[0].deviceId, playerInput);
        }
    
        public static void RemovePlayerInput(int deviceID)
        {
            PlayerInputs.Remove(deviceID);
        }

        public static PlayerInput GetPlayerInput(int deviceID)
        {
            return PlayerInputs[deviceID];
        }

        public static bool TryGetPlayerInput(int deviceID, out PlayerInput playerInput)
        {
            return PlayerInputs.TryGetValue(deviceID, out playerInput);
        }
    }
}
