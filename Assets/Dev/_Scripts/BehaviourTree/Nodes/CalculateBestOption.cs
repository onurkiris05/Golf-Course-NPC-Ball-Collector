using System.Collections.Generic;
using Game.Collectables;
using Game.Managers;
using Game.NPC;
using UnityEngine;
using UnityEngine.AI;

namespace Game.BehaviourTree
{
    public class CalculateBestOption : Node
    {
        private NPCController _controller;
        private bool _isCalculated;

        public CalculateBestOption(NPCController controller)
        {
            _controller = controller;
        }

        private float CalculatePathCost(Vector3 from, Vector3 to)
        {
            var path = new NavMeshPath();
            if (NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path)
            && path.status == NavMeshPathStatus.PathComplete)
            {
                var pathLength = 0f;
                for (var i = 1; i < path.corners.Length; i++)
                    pathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);

                return pathLength * _controller.GetStaminaCostPerUnit();
            }

            return float.MaxValue;
        }


        private List<Collectable_Base> GetAccessibleCollectables()
        {
            List<Collectable_Base> accessible = new();
            foreach (var collectable in _controller.GetCollectables())
            {
                if (CalculatePathCost(_controller.GetAgentPosition(), collectable.transform.position) != float.MaxValue)
                {
                    accessible.Add(collectable);
                }
            }
            return accessible;
        }

        private List<Transform> GenerateChain(GameStyle gameStyle)
        {
            var currentPosition = _controller.GetAgentPosition();
            var accessibleCollectables = GetAccessibleCollectables();
            var chain = new List<Transform>();
            var currentStamina = _controller.GetCurrentStamina();
            var cartTransform = _controller.GetCartTransform();

            while (accessibleCollectables.Count > 0)
            {
                Collectable_Base bestNext = null;
                var bestScore = float.MinValue;

                foreach (var collectable in accessibleCollectables)
                {
                    var toBallCost = CalculatePathCost(currentPosition, collectable.transform.position);
                    var toCartCost = CalculatePathCost(collectable.transform.position, cartTransform.position);
                    var totalCost = toBallCost + toCartCost;

                    if (totalCost > currentStamina)
                        continue;

                    var score = collectable.RewardPoint - totalCost;
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestNext = collectable;
                    }
                }

                if (bestNext == null)
                    break;

                chain.Add(bestNext.transform);
                currentStamina -= CalculatePathCost(currentPosition, bestNext.transform.position);

                switch (gameStyle)
                {
                    case GameStyle.Sequence:
                        chain.Add(cartTransform);
                        currentStamina -= CalculatePathCost(bestNext.transform.position, cartTransform.position);
                        currentPosition = cartTransform.position;
                        break;
                    case GameStyle.Chaining:
                        currentPosition = bestNext.transform.position;
                        break;
                }

                accessibleCollectables.Remove(bestNext);
            }

            if (gameStyle == GameStyle.Chaining)
                chain.Add(cartTransform);

            Debug.Log($"{(gameStyle == GameStyle.Chaining ? "Best chain" : "Sequential chain")} calculated. Chain count: {chain.Count}");
            return chain;
        }

        public override NodeState Evaluate()
        {
            if (_isCalculated)
            {
                _state = NodeState.FAILURE;
                return _state;
            }

            var chain = GenerateChain(_controller.GameStyle);

            if (chain.Count == 0)
            {
                _state = NodeState.FAILURE;
                return _state;
            }

            parent.SetData("chain", chain);
            _isCalculated = true;
            _state = NodeState.SUCCESS;
            return _state;
        }

    }
}
