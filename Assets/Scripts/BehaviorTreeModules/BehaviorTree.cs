using UnityEngine;

namespace BehaviorTreeModules
{
    public abstract class BehaviorTree : MonoBehaviour
    {
        protected Node root;

        protected void Start()
        {
            root = SetupTree();
            Debug.Log("<color=green>Behavior Tree Initialized</color>");

            if (root == null)
            {
                Debug.LogError("<color=red>Behavior Tree root is null</color>");
                return;
            }
        }

        private void Update()
        {
            root?.Evaluate();
        }

        protected abstract Node SetupTree();

        [ContextMenu("show Behavior Tree root node")]
        public void ShowRootNode()
        {
            bool isRootNull = root == null;
            Debug.Log($"<color=yellow>Behavior Tree root node: {isRootNull}</color>");
        }
    }
}