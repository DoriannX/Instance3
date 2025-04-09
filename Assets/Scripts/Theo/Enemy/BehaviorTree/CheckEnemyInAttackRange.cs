using System;
using BehaviorTree;
using UnityEngine;

namespace Enemy.BehaviorTree
{
    public class CheckEnemyInAttackRange : Node
    {
        private readonly float radius;
        private readonly string targetKey;
        
        private readonly Transform transform;
        
        public CheckEnemyInAttackRange(Transform transform, float radius, string targetKey)
        {
            this.radius = radius;
            this.transform = transform;
            this.targetKey = targetKey;
        }
        
        public override NodeState Evaluate()
        {
            object data = GetData(targetKey);
            if (data == null)
                return NodeState.FAILURE;
            
            if (data is not Transform targetTransform)
                throw new InvalidCastException("Target is not a Transform");
            
            return Vector3.Distance(transform.position, targetTransform.position) > radius ? NodeState.FAILURE : NodeState.SUCCESS;
        }
    }
}