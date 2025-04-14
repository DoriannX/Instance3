using System;
using System.Collections.Generic;
using BehaviorTree;
using Theo.Enemy;
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
        [SerializeField] private float chaseStopDistance = 1.5f;
        
        [Header("Patrol")]
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private PatrolMovementType patrolMovementType;
        [SerializeField] private float patrolSpeed = 2f;
        [SerializeField] private float patrolWaitTime = 2f;
        [SerializeField] private float patrolStopDistance = 0.5f;
        
        private readonly string targetKey = "Target";

        protected override Node SetupTree()
        {
            return new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new CheckEnemyInAttackRange(transform, enemyLayer, attackDetectionRadius, targetKey, maxEnemyDetection),
                    new TaskAttackEnemy(attackCooldownTime, attackDamage, targetKey)
                }),
                new Sequence(new List<Node>
                {
                    new CheckEnemyInFOVRange(transform, enemyLayer, fovDetectionRadius, fovAngle, maxEnemyDetection, targetKey),
                    new TaskGoToTarget(navMeshAgent, chaseSpeed, chaseStopDistance, targetKey)
                    
                }),
                new TaskPatrol(patrolPoints, patrolMovementType, navMeshAgent, patrolSpeed, patrolWaitTime, patrolStopDistance),
            });
        }
    }
}