using System;
using BehaviorTreeModules;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Enemy.BehaviorTree
{
    public class WaitToAttack : Node
    {
        private float startTime;
        private float time;
        
        private bool isWaiting;
        
        private readonly float cooldown;
        private readonly string enableAttackKey;
        private readonly string targetKey;
        private readonly NavMeshAgent navMeshAgent;
        private readonly Transform selfTransform;
        
        public WaitToAttack(float cooldown, string enableAttackKey, NavMeshAgent navMeshAgent, string targetKey, Transform selfTransform)
        {
            this.cooldown = cooldown;
            this.enableAttackKey = enableAttackKey;
            this.navMeshAgent = navMeshAgent;
            this.targetKey = targetKey;
            this.selfTransform = selfTransform;

            isWaiting = false;
        }
        
        public override NodeState Evaluate()
        {
            object data = GetData(enableAttackKey);
            
            if (data == null)
                return NodeState.FAILURE;
            
            if (data is not bool enableAttack)
                throw new InvalidCastException("EnableAttack is not a bool");
            
            if (!enableAttack)
            {
                isWaiting = false;
                return NodeState.FAILURE;
            }
            
            if (!isWaiting)
            {
                startTime = Time.time;
                time = startTime;
                isWaiting = true;
            }
            
            if (AgentHasReachedDestination())
            {
                time += Time.deltaTime;
                
                object targetData = GetData(targetKey);

                if (targetData == null)
                    return NodeState.FAILURE;
                
                if (targetData is not Transform targetTransform)
                    throw new InvalidCastException("TargetData is not a Transform");
                
                Vector3 lookAtRotation = Quaternion.LookRotation(targetTransform.position - selfTransform.position).eulerAngles;
                lookAtRotation.x = selfTransform.rotation.eulerAngles.x;
                lookAtRotation.z = selfTransform.rotation.eulerAngles.z;
                selfTransform.rotation = Quaternion.Euler(lookAtRotation);
            }
            
            if (time < startTime + cooldown)
                return NodeState.FAILURE;
            
            isWaiting = false;
            return NodeState.SUCCESS;
        }
        
        private bool AgentHasReachedDestination()
        {
            return !navMeshAgent.pathPending &&
                   navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance &&
                   (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude < 0.1f);
        }
    }
}