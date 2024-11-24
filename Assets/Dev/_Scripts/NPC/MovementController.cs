using System.Collections;
using System.Collections.Generic;
using Game.Collectables;
using Game.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Game.NPC
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] float speed = 3f;
        public float Speed => speed;
        public Vector3 AgentPosition => _agent.transform.position;
        private NPCController _controller;
        private NavMeshAgent _agent;
        private bool _isTriggered;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            _agent.speed = speed;
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

        public void ProcessChain(List<Transform> chain)
        {
            if (_isTriggered) return;

            _isTriggered = true;
            StartCoroutine(ProcessChainCoroutine(chain));
        }

        public float NormalizedSpeed() => _agent.velocity.magnitude / _agent.speed;

        private void OnGameStateChanged(GameState state)
        {
            if (state == GameState.End || state == GameState.Tired)
                _agent.isStopped = true;
        }

        private IEnumerator ProcessChainCoroutine(List<Transform> chain)
        {
            //Collect all collectables
            foreach (var target in chain)
            {
                _agent.SetDestination(target.position);

                while (Vector3.Distance(_agent.transform.position, target.position)
                > _agent.stoppingDistance)
                    yield return null;
            }

            _controller.ProcessGameEnd();
            _isTriggered = false;
            Debug.Log("All collectables collected and returned to the cart!");
        }

    }
}
