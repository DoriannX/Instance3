using System;
using BehaviorTreeModules;
using UnityEngine;

namespace Entities.Enemy.BehaviorTree
{
    public class CheckEnemyInRoom : Node
    {
        private readonly string targetKey;
        
        public CheckEnemyInRoom(string targetKey)
        {
            this.targetKey = targetKey;
        }
        
        public override NodeState Evaluate()
        {
            object data = GetData(targetKey);
            
            if (data == null)
                return NodeState.FAILURE;
            
            if (data is not Transform targetTransform)
                throw new InvalidCastException("Data is not a Transform");
            
            return targetTransform ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}