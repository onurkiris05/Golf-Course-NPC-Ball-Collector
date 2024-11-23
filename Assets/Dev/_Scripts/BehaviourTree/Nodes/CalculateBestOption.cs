using System.Collections.Generic;
using Game.Collectables;
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

        private List<Collectable_Base> FindBestChain()
        {
            var currentPosition = _controller.GetAgentPosition();
            var accessibleCollectables = GetAccessibleCollectables();
            var bestChain = new List<Collectable_Base>();
            var currentStamina = _controller.GetCurrentStamina();
            var cartPosition = _controller.GetBasePoint();

            while (accessibleCollectables.Count > 0)
            {
                Collectable_Base bestNext = null;
                var bestScore = float.MinValue;

                foreach (var collectable in accessibleCollectables)
                {
                    var toBallCost = CalculatePathCost(currentPosition, collectable.transform.position);
                    var toCartCost = CalculatePathCost(collectable.transform.position, cartPosition);
                    var totalCost = toBallCost + toCartCost;

                    if (totalCost > currentStamina)
                        continue;

                    var score = collectable.RewardPoint - toBallCost;
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestNext = collectable;
                    }
                }

                if (bestNext == null)
                    break;

                bestChain.Add(bestNext);
                currentStamina -= CalculatePathCost(currentPosition, bestNext.transform.position);
                currentPosition = bestNext.transform.position;
                accessibleCollectables.Remove(bestNext);
            }

            return bestChain;
        }

        public override NodeState Evaluate()
        {
            if (_isCalculated)
            {
                _state = NodeState.FAILURE;
                return _state;
            }

            var bestChain = FindBestChain();
            if (bestChain.Count < 0)
            {
                _state = NodeState.FAILURE;
                return _state;
            }

            parent.SetData("bestChain", bestChain);
            _isCalculated = true;
            _state = NodeState.SUCCESS;
            return _state;
        }
    }
}
