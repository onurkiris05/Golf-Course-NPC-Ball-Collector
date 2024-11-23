using System.Collections.Generic;
using Game.BehaviourTree;
using Game.Collectables;
using Game.Managers;
using UnityEngine;

namespace Game.NPC
{
    public class NPCController : MonoBehaviour
    {
        [SerializeField] Transform basePoint;
        public bool IsTired { get; private set; } = false;
        public bool IsPlaying { get; private set; } = false;
        private StaminaController _staminaController;
        private AnimationController _animationController;
        private MovementController _movementController;
        private int _collectedScore;

        private void Awake()
        {
            _staminaController = GetComponent<StaminaController>();
            _animationController = GetComponent<AnimationController>();
            _movementController = GetComponent<MovementController>();
        }

        private void OnEnable()
        {
            GameManager.OnBeforeStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            GameManager.OnBeforeStateChanged -= OnGameStateChanged;
        }

        private void Start()
        {
            _animationController.Init(this);
            _movementController.Init(this);
            _staminaController.Init(this);
        }

        public List<Collectable_Base> GetCollectables() => CollectableManager.Instance.SpawnedCollectables;
        public float GetCurrentStamina() => _staminaController.Stamina;
        public float GetStaminaCostPerUnit() => _staminaController.StaminaCostPerUnit;
        public Vector3 GetBasePoint() => basePoint.position;
        public Vector3 GetAgentPosition() => _movementController.AgentPosition;
        public float GetSpeed() => _movementController.Speed;
        public void ProcessOnTired() => GameManager.Instance.ChangeState(GameState.Tired);

        public void ProcessMovement(List<Collectable_Base> bestChain)
        {
            _movementController.ProcessBestChain(bestChain);
            DrawDebugPathWithGizmos(bestChain, GetBasePoint());
        }

        public void ProcessGameEnd()
        {
            GameManager.Instance.ChangeState(GameState.End);
        }

        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Playing:
                    IsPlaying = true;
                    break;
                case GameState.Tired:
                    IsTired = true;
                    break;
                case GameState.Menu:
                    IsTired = true;
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Collectable")
            && other.TryGetComponent<Collectable_Base>(out var collectable))
            {
                _collectedScore += collectable.Collect();
            }
            else if (other.CompareTag("Base"))
            {
                ScoreManager.Instance.OnScoreChanged(_collectedScore);
                _animationController.SetTrigger("isTired");
                _collectedScore = 0;
            }
        }

        #region Debug
        private List<Vector3> debugPathPoints = new List<Vector3>();

        public void DrawDebugPathWithGizmos(List<Collectable_Base> bestChain, Vector3 cartPosition)
        {
            debugPathPoints.Clear();

            if (bestChain == null || bestChain.Count == 0)
            {
                Debug.LogWarning("BestChain listesi boş veya null, çizilecek bir rota yok.");
                return;
            }

            debugPathPoints.Add(cartPosition);

            foreach (var collectable in bestChain)
            {
                debugPathPoints.Add(collectable.transform.position);
            }

            debugPathPoints.Add(cartPosition);
        }

        private void OnDrawGizmos()
        {
            if (debugPathPoints == null || debugPathPoints.Count < 2) return;

            Gizmos.color = Color.green;

            for (int i = 0; i < debugPathPoints.Count - 1; i++)
            {
                Gizmos.DrawLine(debugPathPoints[i], debugPathPoints[i + 1]);
            }

            Gizmos.color = Color.red;
            foreach (var point in debugPathPoints)
            {
                Gizmos.DrawSphere(point, 0.2f);
            }
        }
        #endregion
    }
}
