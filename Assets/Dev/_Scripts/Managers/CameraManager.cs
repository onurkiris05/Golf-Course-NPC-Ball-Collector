using Cinemachine;
using Game.Utilities;
using UnityEngine;

namespace Game.Managers
{
    public class CameraManager : StaticInstance<CameraManager>
    {
        [SerializeField] CinemachineVirtualCamera followCam;
        [SerializeField] CinemachineVirtualCamera closeLookupCam;

        private void OnEnable()
        {
            GameManager.OnBeforeStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            GameManager.OnBeforeStateChanged -= OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState state)
        {
            followCam.Priority = state == GameState.Playing ? 1 : 0;
            closeLookupCam.Priority = state == GameState.Playing ? 0 : 1;
        }
    }
}
