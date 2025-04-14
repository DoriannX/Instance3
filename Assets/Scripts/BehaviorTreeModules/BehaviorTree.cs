using UnityEngine;

namespace BehaviorTreeModules
{
    public abstract class BehaviorTree : MonoBehaviour
    {
        private Node root;

        protected void Start()
        {
            root = SetupTree();
        }

        private void Update()
        {
            root?.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}