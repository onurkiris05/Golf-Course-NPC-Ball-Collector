using System.Collections.Generic;

namespace Game.BehaviourTree
{
    [System.Serializable]
    public abstract class Node
    {
        public Node parent;

        protected NodeState _state;
        protected List<Node> _children = new();

        // Context for storing and sharing data between nodes
        private Dictionary<string, object> _dataContext = new();

        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node child in children)
                Attach(child);
        }

        private void Attach(Node node)
        {
            node.parent = this;
            _children.Add(node);
        }

        public abstract NodeState Evaluate();

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        // Retrieves data using a key, checks parent nodes if not found locally
        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
                return value;

            Node node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;
                node = node.parent;
            }
            return null;
        }

        // Clears data by key, checks parent nodes if not found locally
        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.parent;
            }
            return false;
        }
    }

    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE,
    }
}
