using System.Collections.Generic;
using Game.Collectables;
using Game.NPC;
using UnityEngine;

namespace Game.BehaviourTree
{
    public class Collect : Node
    {
        private NPCController _controller;

        public Collect(NPCController controller)
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            List<Transform> chain = (List<Transform>)GetData("chain");

            if (chain == null || chain.Count <= 0)
            {
                _state = NodeState.FAILURE;
                return _state;
            }

            _controller.ProcessMovement(chain);
            _state = NodeState.RUNNING;
            return _state;
        }
    }
}