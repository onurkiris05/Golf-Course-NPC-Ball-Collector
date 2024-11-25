using Cinemachine;
using Game.Utilities;
using UnityEngine;

namespace Game.Managers
{
    public class CameraManager : StaticInstance<CameraManager>
    {
        [Header("Components")]
        [SerializeField] CinemachineVirtualCamera followCam;
        [SerializeField] CinemachineVirtualCamera closeLookupCam;

        #region UNITY EVENTS
        private void OnEnable()
        {
            GameManager.OnBeforeStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            GameManager.OnBeforeStateChanged -= OnGameStateChanged;
        }
        #endregion

        #region PRIVATE METHODS
        private void OnGameStateChanged(GameState state)
        {
            followCam.Priority = state == GameState.Playing ? 1 : 0;
            closeLookupCam.Priority = state == GameState.Playing ? 0 : 1;
        }
        #endregion
    }
}
