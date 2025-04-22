using System.Collections.Generic;
using System.Linq;
using Entities.Enemy;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyItemsDropper : MonoBehaviour
{
    [SerializeField] private float groundDistance = 1f;
    private RaycastHit[] hitInfos = new RaycastHit[10];

    private List<ItemToDrop> allItemsToDrop;
    private Enemy enemy;
    private Transform enemyTransform;

    private void Awake()
    {
        allItemsToDrop = GetComponentsInChildren<ItemToDrop>().ToList();
        if (allItemsToDrop.Count == 0)
        {
            Debug.LogWarning("No items to drop found on " + gameObject.name);
        }

        enemy = GetComponent<Enemy>();
        enemyTransform = transform;
    }

    private void Start()
    {
        enemy.healthComponent.onDeath += DropItems;
    }

    private void DropItems()
    {
        foreach (ItemToDrop itemDrop in allItemsToDrop)
        {
            itemDrop.DropItem(enemyTransform.position);
        }
    }
}