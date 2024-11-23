using System.Collections;
using System.Collections.Generic;
using Game.Managers;
using UnityEngine;

namespace Game.NPC
{
    public class AnimationController : MonoBehaviour
    {
        private NPCController _controller;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            GameManager.OnBeforeStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            GameManager.OnBeforeStateChanged -= OnGameStateChanged;
        }

        public void Init(NPCController controller)
        {
            _controller = controller;
        }

        public void SetTrigger(string trigger)
        {
            _animator.SetTrigger(trigger);
        }

        private void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Playing:
                    _animator.SetBool("isRunning", true);
                    break;
                case GameState.Tired:
                    _animator.SetTrigger("isTired");
                    break;
            }
        }
    }
}
