using System.Collections.Generic;
using Game.Collectables;
using Game.NPC;

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
            List<Collectable_Base> bestChain = (List<Collectable_Base>)GetData("bestChain");

            if (bestChain == null || bestChain.Count <= 0)
            {
                _state = NodeState.FAILURE;
                return _state;
            }

            _controller.ProcessMovement(bestChain);
            _state = NodeState.RUNNING;
            return _state;
        }
    }
}