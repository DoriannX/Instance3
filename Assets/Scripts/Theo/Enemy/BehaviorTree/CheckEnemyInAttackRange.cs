using System;
using BehaviorTree;
using UnityEngine;

namespace Enemy.BehaviorTree
{
    public class CheckEnemyInAttackRange : Node
    {
        private readonly float radius;
        private readonly string targetKey;
        
        private readonly Transform transform;
        private readonly Collider[] colliders;
        private readonly LayerMask layerAttack;
        private readonly Vector3 boxSize;
        
        
        public CheckEnemyInAttackRange(Transform transform, LayerMask layerAttack, float radius, string targetKey, int maxEnemyDetection)
        {
            this.transform = transform;
            this.layerAttack = layerAttack;
            this.radius = radius;
            this.targetKey = targetKey;
            
            colliders = new Collider[maxEnemyDetection];
            boxSize = new Vector3(radius, radius, radius);
        }
        
        public override NodeState Evaluate()
        {
            object data = GetData(targetKey);
            if (data == null)
                return NodeState.FAILURE;
            
            if (data is not Transform targetTransform)
                throw new InvalidCastException("Target is not a Transform");
            
            var nbEnemy = Physics.OverlapBoxNonAlloc(transform.position + transform.forward * radius, boxSize, colliders, transform.rotation, layerAttack);
            
            if (nbEnemy == 0)
                return NodeState.FAILURE;
            
            for (int i = 0; i < nbEnemy; i++)
            {
                Transform colliderTransform = colliders[i].transform;

                if (colliderTransform == targetTransform)
                    return NodeState.SUCCESS;
            }
            
            return NodeState.FAILURE;
        }
    }
}