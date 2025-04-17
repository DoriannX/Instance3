using BehaviorTreeModules;

namespace Entities.Enemy.BehaviorTree
{
    public class TaskEnableAttack : Node
    {
        private readonly string enableAttackKey;
        
        public TaskEnableAttack(string enableAttackKey)
        {
            this.enableAttackKey = enableAttackKey;
        }
        
        public override NodeState Evaluate()
        {
            parent.parent.SetData(enableAttackKey, true);
            return NodeState.SUCCESS;
        }
    }
}