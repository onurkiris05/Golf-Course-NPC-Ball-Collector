using Game.NPC;

namespace Game.BehaviourTree
{
    public class CheckIfTired : Node
    {
        private NPCController _controller;

        public CheckIfTired(NPCController controller)
        {
            _controller = controller;
        }

        public override NodeState Evaluate()
        {
            _state = _controller.IsTired ? NodeState.SUCCESS : NodeState.FAILURE;
            return _state;
        }
    }
}