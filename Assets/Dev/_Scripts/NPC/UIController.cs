using UnityEngine;

namespace Game.NPC
{
    public class UIController : MonoBehaviour
    {
        private Transform targetCam;

        private void Start() => targetCam = Camera.main.transform;

        private void LateUpdate()
        {
            if (targetCam != null)
                transform.LookAt(transform.position + targetCam.forward);
        }
    }
}
