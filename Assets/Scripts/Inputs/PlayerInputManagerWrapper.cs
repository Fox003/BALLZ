using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class PlayerInputManagerWrapper : MonoBehaviour
    {
        [SerializeField] private PlayerInputManager _playerInputManager;

        private void OnEnable()
        {
            _playerInputManager.onPlayerJoined += OnPlayerJoined;
            _playerInputManager.onPlayerLeft += OnPlayerLeft;
        }
    
        private void OnDisable()
        {
            _playerInputManager.onPlayerJoined -= OnPlayerJoined;
            _playerInputManager.onPlayerLeft -= OnPlayerLeft;
        }

        public void OnPlayerJoined(PlayerInput playerInput)
        {
            PlayerInputRegistry.AddPlayerInput(playerInput);
            Debug.Log("OnPlayerJoined");
        }
    
        public void OnPlayerLeft(PlayerInput playerInput)
        {
            PlayerInputRegistry.RemovePlayerInput(playerInput.user.pairedDevices[0].deviceId);
            Debug.Log("OnPlayerLeft");
        }
    }
}
