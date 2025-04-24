using Item;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using UnityEditor;
using Random = UnityEngine.Random;

namespace Entities.Enemy
{
    public class ItemToDrop : MonoBehaviour
    {
        [Header("Drop Item Settings")] [SerializeField]
        private ItemDrop itemDropPrefab;

        [SerializeField] private uint dropRangeMin;
        [SerializeField] private uint dropRangeMax;
        [SerializeField] private bool isUniqueItem;
        [SerializeField] private float dropChance = 1;

        [Header("Force Settings")] [SerializeField]
        private float forceMagnitudeMin = 700f;

        [SerializeField] private float forceMagnitudeMax = 1500f;
        [SerializeField] private float upwardForceMin;
        [SerializeField] private float upwardForceMax = 500f;

        private static HashSet<string> droppedUniqueItems = new HashSet<string>();

        private void Awake()
        {
            Assert.IsNotNull(itemDropPrefab);
        }

        public void DropItem(Vector3 position)
        {
            bool isUniqueItemAlreadyDropped = isUniqueItem && droppedUniqueItems.Contains(itemDropPrefab.uniqueItemId);
            bool shouldDrop = Random.Range(0f, 1f) < dropChance && !isUniqueItemAlreadyDropped;
            if (!shouldDrop)
            {
                return;
            }

            int dropAmount = Random.Range((int)dropRangeMin, (int)dropRangeMax + 1);

            for (int i = 0; i < dropAmount; i++)
            {
                ItemDrop itemDropInstance = Instantiate(itemDropPrefab, position, Quaternion.identity);
                Rigidbody itemDropInstanceRb = itemDropInstance.GetComponent<Rigidbody>();
                if (itemDropInstanceRb != null)
                {
                    Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                    float forceMagnitude = Random.Range(forceMagnitudeMin, forceMagnitudeMax);
                    float upwardForce = Random.Range(upwardForceMin, upwardForceMax);
                    itemDropInstanceRb.AddForce(randomDirection * forceMagnitude + Vector3.up * upwardForce,
                        ForceMode.Impulse);
                }
            }

            if (isUniqueItem)
            {
                droppedUniqueItems.Add(itemDropPrefab.uniqueItemId);
            }
        }

        public static void ResetUniqueItems()
        {
            droppedUniqueItems.Clear();
        }

        private void OnApplicationQuit()
        {
            ResetUniqueItems();
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += HandlePlayModeStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= HandlePlayModeStateChanged;
        }

        private void HandlePlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                ResetUniqueItems();
            }
        }
#endif
    }
}