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
        private readonly float patrolStopDistance;
        
        private readonly Transform[] patrolPoints;
        private readonly NavMeshAgent navMeshAgent;
        
        public TaskPatrol(Transform[] patrolPoints, NavMeshAgent navMeshAgent, float patrolSpeed, float patrolWaitTime, float patrolStopDistance)
        {
            this.patrolPoints = patrolPoints;
            this.navMeshAgent = navMeshAgent;
            this.patrolWaitTime = patrolWaitTime;
            this.patrolStopDistance = patrolStopDistance;
            this.patrolSpeed = patrolSpeed;
            
            patrolPointIndex = -1;
            patrolWaitTimer = 0f;
        }
        
        public override NodeState Evaluate()
        {
            if (patrolPoints.Length == 0)
                return NodeState.FAILURE;
            
            navMeshAgent.speed = patrolSpeed;
            navMeshAgent.stoppingDistance = patrolStopDistance;

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