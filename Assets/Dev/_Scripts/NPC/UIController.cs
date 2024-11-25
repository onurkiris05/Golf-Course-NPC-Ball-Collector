using UnityEngine;

namespace Game.NPC
{
    public class UIController : MonoBehaviour
    {
        private Transform targetCam;

        #region UNITY EVENTS
        private void Start() => targetCam = Camera.main.transform;

        private void LateUpdate()
        {
            if (targetCam != null)
                transform.LookAt(transform.position + targetCam.forward);
        }
        #endregion
    }
}
