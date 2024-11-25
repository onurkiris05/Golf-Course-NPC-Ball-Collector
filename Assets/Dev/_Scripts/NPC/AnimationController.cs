using Game.Managers;
using UnityEngine;

namespace Game.NPC
{
    public class AnimationController : MonoBehaviour
    {
        private NPCController _controller;
        private Animator _animator;

        #region UNITY EVENTS
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (!_controller.IsPlaying) return;
            _animator.SetFloat("runSpeed", _controller.GetNormalizedSpeed());
        }

        private void OnEnable()
        {
            GameManager.OnBeforeStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            GameManager.OnBeforeStateChanged -= OnGameStateChanged;
        }
        #endregion

        #region PUBLIC METHODS
        public void Init(NPCController controller)
        {
            _controller = controller;
        }

        public void SetTrigger(string trigger)
        {
            _animator.SetTrigger(trigger);
        }
        #endregion

        #region PRIVATE METHODS
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
                case GameState.End:
                    _animator.SetTrigger("isTired");
                    break;
            }
        }
        #endregion
    }
}
