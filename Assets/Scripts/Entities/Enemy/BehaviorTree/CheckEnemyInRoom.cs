using System;
using BehaviorTreeModules;
using UnityEngine;

namespace Entities.Enemy.BehaviorTree
{
    public class CheckEnemyInRoom : Node
    {
        private readonly string targetKey;
        private readonly string roomDataKey;
        
        public CheckEnemyInRoom(string targetKey, string roomDataKey)
        {
            this.targetKey = targetKey;
            this.roomDataKey = roomDataKey;
        }
        
        public override NodeState Evaluate()
        {
            object data = GetData(roomDataKey);
            
            if (data == null)
                return NodeState.FAILURE;
            
            if (data is not Transform targetTransform)
                throw new InvalidCastException("Data is not a Transform");

            if (!targetTransform)
                return NodeState.FAILURE;
            
            parent.parent.SetData(targetKey, targetTransform);
            return NodeState.SUCCESS;

        }
    }
}