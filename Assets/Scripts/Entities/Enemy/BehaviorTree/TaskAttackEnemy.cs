using System;
using BehaviorTreeModules;
using UnityEngine;

namespace Entities.Enemy.BehaviorTree
{
    public class TaskAttackEnemy : Node
    {
        private readonly float cooldownTime;
        private readonly string targetKey;
        private readonly string enableAttackKey;
        
        private readonly Weapon weapon;
        private readonly Transform selfTransform;
        
        private float lastAttackTime;
        
        public TaskAttackEnemy(Transform transform, Weapon weapon, float cooldownTime, string targetKey, string enableAttackKey)
        {
            this.cooldownTime = cooldownTime;
            this.targetKey = targetKey;
            this.enableAttackKey = enableAttackKey;
            
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
            
            if (data is not Transform target)
                throw new InvalidCastException("Target is not a Transform");

            Vector3 lookAtRotation = Quaternion.LookRotation(target.position - selfTransform.position).eulerAngles;
            lookAtRotation.x = selfTransform.rotation.eulerAngles.x;
            lookAtRotation.z = selfTransform.rotation.eulerAngles.z;
            selfTransform.rotation = Quaternion.Euler(lookAtRotation);
            
            weapon?.Attack(selfTransform);
            lastAttackTime = Time.time;
            parent.parent.SetData(enableAttackKey, false);
            
            return NodeState.SUCCESS;
        }

        private bool CheckAttackTime()
        {
            return Time.time > lastAttackTime + cooldownTime;
        }
    }
}