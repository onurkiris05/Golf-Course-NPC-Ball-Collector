using System.Collections.Generic;

namespace Game.BehaviourTree
{
    // Sequence node: Executes child nodes sequentially
    // Succeeds if all children succeed, fails if any child fails
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            bool anyChildRunning = false;

            // Iterate through each child and evaluate its state
            foreach (var child in _children)
            {
                switch (child.Evaluate())
                {
                    case NodeState.RUNNING:
                        anyChildRunning = true;
                        break;
                    case NodeState.SUCCESS:
                        break;
                    case NodeState.FAILURE:
                        _state = NodeState.FAILURE;
                        return _state;
                }
            }

            _state = anyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return _state;
        }
    }
}