using System;
using BehaviorTreeModules;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Enemy.BehaviorTree
{
    public class TaskLookAt : Node
    {
        private readonly NavMeshAgent navMeshAgent;
        private readonly Transform selfTransform;
        
        private readonly float getCloserDistance = 2;
        private readonly float speed = 1;
        private readonly string targetKey;

        public TaskLookAt(Transform selfTransform, NavMeshAgent navMeshAgent, string targetKey)
        {
            this.selfTransform = selfTransform;
            this.navMeshAgent = navMeshAgent;
            this.targetKey = targetKey;
        }
        
        public override NodeState Evaluate()
        {
            if (AgentHasReachedDestination())
                return NodeState.SUCCESS;
            
            object data = GetData(targetKey);
            
            if (data == null)
                return NodeState.FAILURE;
            
            if (data is not Transform targetTransform)
                throw new InvalidCastException("Target transform is not a Transform");
            
            Vector3 direction = (targetTransform.position - selfTransform.position).normalized;
            
            Debug.Log("<color=red>TaskLookAt</color>");
            
            navMeshAgent.SetDestination(selfTransform.position + direction * getCloserDistance);
            navMeshAgent.speed = speed;
            navMeshAgent.stoppingDistance = 0;
            
            return AgentHasReachedDestination() ? NodeState.SUCCESS : NodeState.RUNNING;
        }
        
        private bool AgentHasReachedDestination()
        {
            return !navMeshAgent.pathPending &&
                   navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance &&
                   (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude < 0.1f);
        }
    }
}