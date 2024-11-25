using System.Collections.Generic;
using Game.NPC;

namespace Game.BehaviourTree
{
    public class NPCBehaviourTree : Tree
    {
        protected override Node SetupTree()
        {
            // Create individual behavior nodes
            var checkIfTired = new CheckIfTired(_controller);
            var calculateBestOption = new CalculateBestOption(_controller);
            var collect = new Collect(_controller);

            // Define a sequence for the 'search' behavior
            Sequence search = new Sequence(new List<Node>{
                calculateBestOption,
                collect});

            // Define the root node as a selector with 'checkIfTired' and 'search'
            Selector root = new Selector(new List<Node> { checkIfTired, search });

            return root;
        }
    }
}
