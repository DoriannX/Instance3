using BehaviorTree;
using Entities.Enemy.BehaviorTree.Modes;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Enemy.BehaviorTree
{
    public class TaskPatrol : Node
    {
        private float patrolWaitTimer;
        private int patrolPointIndex;

        private NavMeshPath currentPath;
        
        private readonly float patrolWaitTime;
        private readonly float patrolSpeed;
        private readonly float patrolStopDistance;
        
        private readonly Transform[] patrolPoints;
        private readonly NavMeshAgent navMeshAgent;
        
        private readonly PatrolMovementType patrolMovementType;
        private int patrolPointIndexIncrementer = 1;
        
        public TaskPatrol(Transform[] patrolPoints, PatrolMovementType patrolMovementType, NavMeshAgent navMeshAgent, float patrolSpeed, float patrolWaitTime, float patrolStopDistance)
        {
            this.patrolPoints = patrolPoints;
            this.navMeshAgent = navMeshAgent;
            this.patrolMovementType = patrolMovementType;
            this.patrolWaitTime = patrolWaitTime;
            this.patrolStopDistance = patrolStopDistance;
            this.patrolSpeed = patrolSpeed;
            
            patrolPointIndex = 0;
            patrolWaitTimer = 0f;
        }
        
        public override NodeState Evaluate()
        {
            if (patrolPoints.Length == 0)
                return NodeState.FAILURE;
            
            if (currentPath != navMeshAgent.path)
            {
                navMeshAgent.speed = patrolSpeed;
                navMeshAgent.stoppingDistance = patrolStopDistance;
                SetDestination(patrolPoints[patrolPointIndex].position);
            }

            if (AgentHasReachedDestination())
            {
                patrolWaitTimer += Time.deltaTime;
                
                if (patrolWaitTimer >= patrolWaitTime)
                {
                    IncreasePatrolPointIndex();
                    SetDestination(patrolPoints[patrolPointIndex].position);

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

        private void IncreasePatrolPointIndex()
        {
            switch (patrolMovementType)
            {
                case PatrolMovementType.Loop :
                {
                    patrolPointIndex = ++patrolPointIndex % patrolPoints.Length;
                    break;
                }
                case PatrolMovementType.BackAndForth :
                {
                    if (patrolPointIndex == patrolPoints.Length - 1)
                        patrolPointIndexIncrementer = -1;
                    else if (patrolPointIndex == 0)
                        patrolPointIndexIncrementer = 1;

                    patrolPointIndex += patrolPointIndexIncrementer;
                    break;
                }
            }
        }
        
        private void SetDestination(Vector3 destination)
        {
            if (navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.SetDestination(destination);
                currentPath = navMeshAgent.path;
            }
        }
    }
}