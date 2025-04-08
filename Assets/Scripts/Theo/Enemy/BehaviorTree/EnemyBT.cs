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
        
        [Header("Patrol")]
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private float patrolSpeed = 2f;
        [SerializeField] private float patrolWaitTime = 2f;
        
        
        protected override Node SetupTree()
        {
            return new Selector(new List<Node>
            {
                new TaskPatrol(patrolPoints, patrolSpeed, patrolWaitTime, navMeshAgent),
            });
        }
    }
}