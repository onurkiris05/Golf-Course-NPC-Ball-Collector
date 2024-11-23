using Game.NPC;
using UnityEngine;

namespace Game.BehaviourTree
{
    public abstract class Tree : MonoBehaviour
    {
        protected NPCController _controller;

        private void Awake()
        {
            _controller = GetComponent<NPCController>();
        }

        private Node _root;

        protected void Start()
        {
            _root = SetupTree();
        }

        protected void Update()
        {
            if (!_controller.IsPlaying) return;
            _root?.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}