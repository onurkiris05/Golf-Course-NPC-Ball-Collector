using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.NPC
{
    public class StaminaController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float staminaCostPerUnit = 0.1f;
        [SerializeField] private float stamina = 100f;
        private NPCController _controller;
        private Vector3 _lastPosition;

        public float StaminaCostPerUnit => staminaCostPerUnit;
        public float Stamina => stamina;

        private void Start() => _lastPosition = transform.position;

        private void Update()
        {
            if (!_controller.IsPlaying) return;

            var distanceTraveled = Vector3.Distance(transform.position, _lastPosition);
            ConsumeStaminaBasedOnDistance(distanceTraveled);
            _lastPosition = transform.position;
        }

        public void Init(NPCController controller)
        {
            _controller = controller;
        }

        private void ConsumeStaminaBasedOnDistance(float distance)
        {
            var staminaLoss = distance * staminaCostPerUnit;
            stamina -= staminaLoss;

            if (stamina < 0f)
            {
                stamina = 0f;
                _controller.ProcessOnTired();
            }
        }
    }
}
