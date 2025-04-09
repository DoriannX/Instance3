using System;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using Tree = BehaviorTree.Tree;

namespace Enemy.BehaviorTree
{
    public sealed class EnemyBT : Tree
    {
        [Header("General")]
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private LayerMask enemyLayer;
        
        [Header("Detection")]
        [SerializeField] private float fovDetectionRadius = 10f;
        [SerializeField] private float fovAngle = 90f;
        [SerializeField] private int maxEnemyDetection = 10;
        
        [Header("Patrol")]
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private float patrolSpeed = 2f;
        [SerializeField] private float patrolWaitTime = 2f;
        
        [Header("Chase")]
        [SerializeField] private float chaseSpeed = 3.5f;
        
        [Header("NavMeshAgent")]
        [SerializeField] private float stoppingDistance = 0.5f;
        
        private readonly string targetKey = "Target";

        protected override Node SetupTree()
        {
            SetupNavMeshAgent();
            
            return new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new CheckEnemyInAttackRange(transform, stoppingDistance, targetKey),
                    new TaskAttackEnemy(targetKey)
                }),
                new Sequence(new List<Node>
                {
                    new CheckEnemyInFOVRange(transform, enemyLayer, fovDetectionRadius, fovAngle, maxEnemyDetection, targetKey),
                    new TaskGoToTarget(navMeshAgent, chaseSpeed, targetKey)
                    
                }),
                new TaskPatrol(patrolPoints, patrolSpeed, patrolWaitTime, navMeshAgent),
            });
        }

        private void SetupNavMeshAgent()
        {
            if (!navMeshAgent)
                throw new MissingFieldException("NavMeshAgent is not assigned");

            navMeshAgent.stoppingDistance = stoppingDistance;
        }
    }
}