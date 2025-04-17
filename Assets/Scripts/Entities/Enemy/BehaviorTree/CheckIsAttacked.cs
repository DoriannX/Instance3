using System;
using BehaviorTreeModules;
using UnityEngine;

namespace Entities.Enemy.BehaviorTree
{
    public class CheckIsAttacked : Node
    {
        private readonly float delay;
        private readonly string dataKey;
        private readonly string targetKey;
        
        public CheckIsAttacked(float delay, string dataKey, string targetKey)
        {
            this.delay = delay;
            this.dataKey = dataKey;
            this.targetKey = targetKey;
        }
        
        
        public override NodeState Evaluate()
        {
            object data = GetData(dataKey);
            
            if (data == null)
                return NodeState.FAILURE;
            
            if (data is not (float lastTimeAttacked, Transform origin))
                throw new InvalidCastException("Data is not a tuple of (float, Transform)");
            
            /*
            Debug.Log($"Time.time: {Time.time}");
            Debug.Log($"lastTimeAttacked: {lastTimeAttacked}");
            Debug.Log($"delay: {delay}");
            Debug.Log($"Time.time < lastTimeAttacked + delay {Time.time < lastTimeAttacked - delay}");
            */


            if (Time.time > lastTimeAttacked + delay) 
                return NodeState.FAILURE;
            
            parent.parent.SetData(targetKey, origin);
            return NodeState.SUCCESS;

            //return Time.time < lastTimeAttacked + delay ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}