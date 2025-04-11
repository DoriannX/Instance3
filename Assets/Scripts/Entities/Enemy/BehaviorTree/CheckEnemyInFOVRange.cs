using System;
using BehaviorTree;
using UnityEngine;

namespace Entities.Enemy.BehaviorTree
{
    public class CheckEnemyInFOVRange : Node
    {
        private readonly float detectionRadius;
        private readonly float fovAngle;
        private readonly string targetKey;
        
        private readonly Collider[] colliders;
        private readonly Transform transform;
        private readonly LayerMask layerMask;
        
        public CheckEnemyInFOVRange(Transform transform, LayerMask layer, float detectionRadius, float fovAngle, int maxEnemyDetection, string targetKey)
        {
            colliders = new Collider[maxEnemyDetection];
            layerMask = layer;
            
            this.transform = transform;
            this.detectionRadius = detectionRadius;
            this.fovAngle = fovAngle;
            
            this.targetKey = targetKey;
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
                    if (IsInFOV(targetTransform) && IsVisible(targetTransform))
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
            
            int count = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, colliders, layerMask);
            
            if (count <= 0)
                return NodeState.FAILURE;

            for (int i = 0; i < count; i++)
            {
                Transform targetTransform = colliders[i].transform;
                
                if (!IsInFOV(targetTransform))
                    continue;

                if (!IsVisible(targetTransform)) 
                    continue;
                
                parent.parent.SetData(targetKey, colliders[i].transform);
                return NodeState.SUCCESS;

            }

            return NodeState.FAILURE;
        }

        private bool IsInFOV(Transform target)
        {
            Vector3 directionToTarget = target.position - transform.position;
            float angle = Vector3.Angle(transform.forward, directionToTarget);
            return angle < fovAngle / 2;
        }

        private bool IsVisible(Transform target)
        {
            if (Physics.Linecast(transform.position, target.position, out RaycastHit hit))
            {
                return hit.transform == target;
            }

            return false;
        }
    }
}