using System.Collections;
using System.Collections.Generic;
using Game.Collectables;
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

        public void Init(NPCController controller)
        {
            _controller = controller;
        }

        public void ProcessBestChain(List<Collectable_Base> bestChain)
        {
            if (_isTriggered) return;

            _isTriggered = true;
            StartCoroutine(ProcessChainCoroutine(bestChain));
        }

        private IEnumerator ProcessChainCoroutine(List<Collectable_Base> bestChain)
        {
            //Collect all collectables
            foreach (var collectable in bestChain)
            {
                _agent.SetDestination(collectable.transform.position);

                while (Vector3.Distance(_agent.transform.position, collectable.transform.position)
                > _agent.stoppingDistance)
                    yield return null;
            }

            // Return to the base
            var basePosition = _controller.GetBasePoint();
            _agent.SetDestination(basePosition);

            while (Vector3.Distance(_agent.transform.position, basePosition) > _agent.stoppingDistance)
                yield return null;

            _controller.ProcessGameEnd();
            _isTriggered = false;
            Debug.Log("All collectables collected and returned to the cart!");
        }

    }
}
