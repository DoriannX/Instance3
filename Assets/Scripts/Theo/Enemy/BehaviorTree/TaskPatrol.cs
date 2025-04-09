using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.BehaviorTree
{
    public class TaskPatrol : Node
    {
        private float patrolWaitTimer;
        private int patrolPointIndex;
        
        private readonly float patrolWaitTime;
        private readonly float patrolSpeed;
        
        private readonly Transform[] patrolPoints;
        private readonly NavMeshAgent navMeshAgent;
        
        public TaskPatrol(Transform[] patrolPoints, float patrolSpeed, float patrolWaitTime, NavMeshAgent navMeshAgent)
        {
            this.patrolPoints = patrolPoints;
            this.patrolWaitTime = patrolWaitTime;
            this.navMeshAgent = navMeshAgent;
            this.patrolSpeed = patrolSpeed;
            
            patrolPointIndex = 0;
            patrolWaitTimer = 0f;
        }
        
        public override NodeState Evaluate()
        {
            if (patrolPoints.Length == 0)
                return NodeState.FAILURE;
            
            navMeshAgent.speed = patrolSpeed;

            if (AgentHasReachedDestination())
            {
                patrolWaitTimer += Time.deltaTime;
                
                if (patrolWaitTimer >= patrolWaitTime)
                {
                    patrolPointIndex = ++patrolPointIndex % patrolPoints.Length;
                    navMeshAgent.SetDestination(patrolPoints[patrolPointIndex].position);
                    patrolWaitTimer = 0f;
                }
                
            }

            return NodeState.RUNNING;
        }

        private bool AgentHasReachedDestination()
        {
            return !navMeshAgent.pathPending &&
                   navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance &&
                   (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude < 0.01f);
        }

    }
}