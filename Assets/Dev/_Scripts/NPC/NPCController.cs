using System.Collections.Generic;
using Game.Collectables;
using Game.Managers;
using Game.Utilities;
using NaughtyAttributes;
using UnityEngine;

namespace Game.NPC
{
    public class NPCController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] Transform basePoint;
        [SerializeField] ParticleSystem cartVFX;

        [Header("Debug")]
        [ShowNonSerializedField]
        private int _collectedScore;

        public bool IsTired { get; private set; } = false;
        public bool IsPlaying { get; private set; } = false;
        public GameStyle GameStyle => _gameStyle;
        private GameStyle _gameStyle;
        private StaminaController _staminaController;
        private AnimationController _animationController;
        private MovementController _movementController;

        #region UNITY EVENTS
        private void Awake()
        {
            _staminaController = GetComponent<StaminaController>();
            _animationController = GetComponent<AnimationController>();
            _movementController = GetComponent<MovementController>();
        }

        private void OnEnable()
        {
            GameManager.OnBeforeStateChanged += OnGameStateChanged;
            GameManager.OnGameStyleChanged += OnGameStyleChanged;
        }

        private void OnDisable()
        {
            GameManager.OnBeforeStateChanged -= OnGameStateChanged;
            GameManager.OnGameStyleChanged -= OnGameStyleChanged;
        }

        private void Start()
        {
            _animationController.Init(this);
            _movementController.Init(this);
            _staminaController.Init(this);
            _gameStyle = GameManager.Instance.GameStyle;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Collectable")
            && other.TryGetComponent<Collectable_Base>(out var collectable))
            {
                _collectedScore += collectable.Collect();
                Spawner_Text.Instance.SpawnAndFade(
                    "Floating Text",
                    $"Collected: +{collectable.RewardPoint}",
                    collectable.transform.position,
                    1f);
            }
            else if (other.CompareTag("Base"))
            {
                if (_collectedScore == 0) return;
                ScoreManager.Instance.OnScoreChanged(_collectedScore);
                cartVFX.Play();
                _collectedScore = 0;
            }
        }
        #endregion

        #region PUBLIC METHODS
        public List<Collectable_Base> GetCollectables() => CollectableManager.Instance.SpawnedCollectables;
        public float GetCurrentStamina() => _staminaController.Stamina;
        public float GetStaminaCostPerUnit() => _staminaController.StaminaCostPerUnit;
        public Transform GetCartTransform() => basePoint;
        public Vector3 GetAgentPosition() => _movementController.AgentPosition;
        public float GetSpeed() => _movementController.Speed;
        public float GetNormalizedSpeed() => _movementController.NormalizedSpeed();
        public void ProcessOnTired() => GameManager.Instance.ChangeState(GameState.Tired);

        public void ProcessMovement(List<Transform> bestChain)
        {
            _movementController.ProcessChain(bestChain);
            DrawDebugPathWithGizmos(bestChain, GetCartTransform());
        }

        public void ProcessGameEnd()
        {
            GameManager.Instance.ChangeState(GameState.End);
        }
        #endregion

        #region PRIVATE METHODS
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
                case GameState.End:
                    IsTired = true;
                    break;
            }
        }

        private void OnGameStyleChanged(GameStyle style)
        {
            _gameStyle = style;
        }
        #endregion

        #region DEBUG 
        private List<Vector3> debugPathPoints = new List<Vector3>();

        public void DrawDebugPathWithGizmos(List<Transform> chain, Transform cartTransform)
        {
            debugPathPoints.Clear();

            if (chain == null || chain.Count == 0)
            {
                Debug.LogWarning("BestChain listesi boş veya null, çizilecek bir rota yok.");
                return;
            }

            debugPathPoints.Add(cartTransform.position);

            foreach (var collectable in chain)
                debugPathPoints.Add(collectable.position);

            debugPathPoints.Add(cartTransform.position);
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
