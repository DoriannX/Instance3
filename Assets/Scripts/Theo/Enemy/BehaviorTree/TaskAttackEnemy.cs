using System;
using BehaviorTree;
using Theo.Enemy;
using UnityEngine;

namespace Enemy.BehaviorTree
{
    public class TaskAttackEnemy : Node
    {
        private readonly float cooldownTime;
        private readonly float damage;
        private readonly string targetKey;
        
        private Transform previousTargetTransform;
        private EntityManager previousTargetEntityManager;
        
        private float lastAttackTime;
        
        public TaskAttackEnemy(float cooldownTime, float damage, string targetKey)
        {
            this.cooldownTime = cooldownTime;
            this.damage = damage;
            this.targetKey = targetKey;
            
            lastAttackTime = Time.time;
        }
        
        public override NodeState Evaluate()
        {
            if (!CheckAttackTime())
                return NodeState.FAILURE;
            
            object data = GetData(targetKey);
            
            if (data == null)
                return NodeState.FAILURE;
            
            if (data is not Transform target)
                throw new InvalidCastException("Target is not a Transform");

            if (target == previousTargetTransform)
            {
                Attack();
                return NodeState.SUCCESS;
            }
            
            if (!target.TryGetComponent(out EntityManager entityManager))
                throw new InvalidCastException("Target does not have EntityManager component");
            
            previousTargetTransform = target;
            previousTargetEntityManager = entityManager;
            
            Attack();
            
            return NodeState.SUCCESS;
        }

        private void Attack()
        {
            previousTargetEntityManager.TakeDamage(damage);
            lastAttackTime = Time.time;
        }

        private bool CheckAttackTime()
        {
            return Time.time > lastAttackTime + cooldownTime;
        }
    }
}