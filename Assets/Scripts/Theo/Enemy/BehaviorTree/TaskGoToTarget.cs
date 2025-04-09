using System;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.BehaviorTree
{
    public class TaskGoToTarget : Node
    {
        private readonly NavMeshAgent navMeshAgent;
        private readonly float speed;
        private readonly float stopDistance;
        private readonly string targetKey;
 
        public TaskGoToTarget(NavMeshAgent navMeshAgent, float speed, float stopDistance, string targetKey)
        {
            this.navMeshAgent = navMeshAgent;
            this.speed = speed;
            this.stopDistance = stopDistance;
            this.targetKey = targetKey;
        }
        
        public override NodeState Evaluate()
        {
            object data = GetData(targetKey);
            
            if (data == null)
                return NodeState.FAILURE;
            
            if (data is not Transform target)
                throw new InvalidCastException("Target is not a Transform");
            
            navMeshAgent.SetDestination(target.position);
            navMeshAgent.speed = speed;
            navMeshAgent.stoppingDistance = stopDistance;
            
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