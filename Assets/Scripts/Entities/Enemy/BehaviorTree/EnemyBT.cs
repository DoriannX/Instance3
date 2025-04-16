using System;
using System.Collections.Generic;
using BehaviorTreeModules;
using Entities.Enemy.BehaviorTree.Modes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace Entities.Enemy.BehaviorTree
{
    public sealed class EnemyBT : BehaviorTreeModules.BehaviorTree
    {
        [Header("General")]
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private LayerMask enemyLayer;
        
        /* -
        [Header("Fight Back")]
        [SerializeField] private float fightBackDelay = 0.5f;
        
        [Header("Audio")]
        [SerializeField] private float audioRange;
        [SerializeField] private float audioThreshold;
        */
        
        [Header("Attack")]
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private Weapon weapon;
        
        [Header("FOV Detection")]
        [SerializeField] private float fovDetectionRadius = 10f;
        [SerializeField] private float fovAngle = 90f;
        [SerializeField] private int maxEnemyDetection = 10;
        
        [Header("Chase")]
        [SerializeField] private float chaseSpeed = 3.5f;
        
        [Header("Patrol")]
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private PatrolMovementType patrolMovementType;
        [SerializeField] private float patrolSpeed = 2f;
        [SerializeField] private float patrolWaitTime = 2f;
        [SerializeField] private float patrolStopDistance = 0.5f;
        
        private readonly string targetKey = "Target";
        //private readonly string attackedKey = "IsAttacked";
        
        private AttackMode attackMode;

        protected override Node SetupTree()
        {
            Assert.IsNotNull(navMeshAgent, "NavMeshAgent is not assigned in the inspector.");
            Assert.IsTrue(patrolPoints.Length > 0, "Patrol points are not assigned in the inspector.");
            Assert.IsNotNull(weapon, "Weapon is not assigned in the inspector.");
            Assert.IsNotNull(weaponData, "WeaponData is not assigned in the inspector.");
            
            SetWeaponData(weaponData);
            
            return CreateTree();
        }

        private Node CreateTree()
        {
            return new Selector(new List<Node>
            {
                new Sequence(new List<Node> // sequence for the attack
                {
                    new CheckEnemyInAttackRange(transform, enemyLayer, attackMode, weaponData.attackRange, targetKey, maxEnemyDetection),
                    new TaskAttackEnemy(transform, weapon, weaponData.cooldown, targetKey)
                }),
                new Sequence(new List<Node> // sequence for the chase
                {
                    new CheckEnemyInRoom(targetKey),
                    new TaskGoToTarget(navMeshAgent, chaseSpeed, weaponData.attackRange, targetKey)
                }),
                new TaskPatrol(patrolPoints, patrolMovementType, navMeshAgent, patrolSpeed, patrolWaitTime, patrolStopDistance),
            });
        }
        
        public void SetWeaponData(WeaponData data)
        {
            if (!data)
                throw new ArgumentNullException(nameof(data), "WeaponData is null.");

            weaponData = data;

            if (weaponData is RangeWeaponData)
                attackMode = AttackMode.Range;
            else if (weaponData is MeleeWeaponData)
                attackMode = AttackMode.Melee;
            else
                throw new ArgumentException("Invalid weapon data type.");
            
            weapon.LoadData(weaponData);
        }

        public void SetTargetInRoom(Transform target)
        {
            root.SetData(targetKey, target);
        }

        /* -
        private void BindEvents()
        {
            if (TryGetComponent(out EntityHealth entityHealth))
            {
                entityHealth.onHit += OnHit;
            }
        }

        private void OnHit(Transform origin)
        {
            root.SetData(attackedKey, (Time.time, origin));
        }*/
    }
}