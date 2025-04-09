using System;
using BehaviorTree;
using Theo.Enemy;
using UnityEngine;

namespace Enemy.BehaviorTree
{
    public class TaskAttackEnemy : Node
    {
        private readonly string targetKey;
        
        private Transform previousTargetTransform;
        private EntityManager previousTargetEntityManager;
        
        public TaskAttackEnemy(string targetKey)
        {
            this.targetKey = targetKey;
        }
        
        public override NodeState Evaluate()
        {
            object data = GetData(targetKey);
            
            if (data == null)
                return NodeState.FAILURE;
            
            if (data is not Transform target)
                throw new InvalidCastException("Target is not a Transform");

            if (target == previousTargetTransform)
            {
                previousTargetEntityManager.TakeDamage(1f);
                return NodeState.SUCCESS;
            }
            
            if (!target.TryGetComponent(out EntityManager entityManager))
                throw new InvalidCastException("Target does not have EntityManager component");
            
            previousTargetTransform = target;
            previousTargetEntityManager = entityManager;
            
            previousTargetEntityManager.TakeDamage(1f);
            
            return NodeState.SUCCESS;
        }
    }
}