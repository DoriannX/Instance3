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
        
        [Header("Attack Detection")]
        [SerializeField] private float attackDetectionRadius = 1.5f;
        
        [Header("Attack")]
        [SerializeField] private float attackDamage = 1f;
        [SerializeField] private float attackCooldownTime = 1f;
        
        [Header("FOV Detection")]
        [SerializeField] private float fovDetectionRadius = 10f;
        [SerializeField] private float fovAngle = 90f;
        [SerializeField] private int maxEnemyDetection = 10;
        
        [Header("Chase")]
        [SerializeField] private float chaseSpeed = 3.5f;
        
        [Header("Patrol")]
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private float patrolSpeed = 2f;
        [SerializeField] private float patrolWaitTime = 2f;
        
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
                    new CheckEnemyInAttackRange(transform, attackDetectionRadius, targetKey),
                    new TaskAttackEnemy(attackCooldownTime, attackDamage, targetKey)
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