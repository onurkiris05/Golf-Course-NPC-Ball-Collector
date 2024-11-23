using System.Collections.Generic;
using Game.NPC;

namespace Game.BehaviourTree
{
    public class NPCBehaviourTree : Tree
    {
        protected override Node SetupTree()
        {
            var checkIfTired = new CheckIfTired(_controller);
            var calculateBestOption = new CalculateBestOption(_controller);
            var collect = new Collect(_controller);

            Sequence search = new Sequence(new List<Node>{
                calculateBestOption,
                collect});

            Selector root = new Selector(new List<Node> { checkIfTired, search });

            return root;
        }
    }
}
