using System.Collections.Generic;

namespace Game.BehaviourTree
{
    // Selector node: Tries child nodes in order
    // Succeeds if any child succeeds, continues if a child is running,
    // and fails only if all children fail
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            // Iterate through each child and evaluate its state
            foreach (var child in _children)
            {
                switch (child.Evaluate())
                {
                    case NodeState.RUNNING:
                        _state = NodeState.RUNNING;
                        return _state;
                    case NodeState.SUCCESS:
                        _state = NodeState.SUCCESS;
                        return _state;
                    case NodeState.FAILURE:
                        break;
                }
            }

            _state = NodeState.FAILURE;
            return _state;
        }
    }
}
