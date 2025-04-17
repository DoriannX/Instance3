using System;
using BehaviorTreeModules;
using UnityEngine;

namespace Entities.Enemy.BehaviorTree
{
    public class CheckEnemyInAudioRange : Node
    {
        private readonly float audioRange;
        private readonly float audioThreshold;
        
        private readonly string targetKey;
        
        private readonly Transform selfTransform;
        private readonly Collider[] colliders;
        private readonly LayerMask layerMask;

        public CheckEnemyInAudioRange(Transform selfTransform, float audioRange, float audioThreshold, string targetKey, int maxEnemyDetection, LayerMask layerMask)
        {
            this.selfTransform = selfTransform;
            this.layerMask = layerMask;
            this.audioRange = audioRange;
            this.audioThreshold = audioThreshold;
            this.targetKey = targetKey;

            colliders = new Collider[maxEnemyDetection];
        }
        
        public override NodeState Evaluate()
        {
            object data = GetData(targetKey);
            
            if (data != null)
            {
                if (data is not Transform targetTransform)
                    throw new InvalidCastException("Data is not a Transform");

                if (targetTransform)
                {
                    if (IsEnoughNoisy(targetTransform))
                    {
                        parent.parent.SetData(targetKey, targetTransform);
                        return NodeState.SUCCESS;
                    }
                }
                else
                {
                    ClearData(targetKey);
                }
            }
            
            int count = Physics.OverlapSphereNonAlloc(selfTransform.position, audioRange, colliders, layerMask);
            
            if (count <= 0)
                return NodeState.FAILURE;

            for (int i = 0; i < count; i++)
            {
                Transform targetTransform = colliders[i].transform;
                
                if (IsEnoughNoisy(targetTransform))
                {
                    parent.parent.SetData(targetKey, targetTransform);
                    return NodeState.SUCCESS;
                }
            }
            
            return NodeState.FAILURE;
        }

        private bool IsEnoughNoisy(Transform target)
        {
            return target.TryGetComponent(out Rigidbody rbTarget) && rbTarget.linearVelocity.magnitude >= audioThreshold;
        }
    }
}