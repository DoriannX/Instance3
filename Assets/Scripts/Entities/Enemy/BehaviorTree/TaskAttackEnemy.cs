using System;
using BehaviorTree;
using UnityEngine;

namespace Entities.Enemy.BehaviorTree
{
    public class TaskAttackEnemy : Node
    {
        private readonly float cooldownTime;
        private readonly float damage;
        private readonly string targetKey;
        
        /*private Transform previousTargetTransform;
        private EntityManager previousTargetEntityManager;*/
        private readonly Weapon weapon;
        private readonly Transform selfTransform;
        
        private float lastAttackTime;
        
        public TaskAttackEnemy(Transform transform, Weapon weapon, float cooldownTime, float damage, string targetKey)
        {
            this.cooldownTime = cooldownTime;
            this.damage = damage;
            this.targetKey = targetKey;
            selfTransform = transform;
            this.weapon = weapon;
            
            lastAttackTime = Time.time;
        }
        
        public override NodeState Evaluate()
        {
            if (!CheckAttackTime())
                return NodeState.FAILURE;
            
            object data = GetData(targetKey);
            
            if (data == null)
                return NodeState.FAILURE;
            
            weapon?.Attack(selfTransform);
            lastAttackTime = Time.time;
            
            return NodeState.SUCCESS;
        }

        /*private void Attack()
        {
            if (previousTargetEntityManager.TakeDamage(damage))
                ClearData(targetKey);
            
            lastAttackTime = Time.time;
        }*/

        private bool CheckAttackTime()
        {
            return Time.time > lastAttackTime + cooldownTime;
        }
    }
}