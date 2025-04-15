using Item.Drops;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Fragment Chip Drop")]
    [SerializeField, Min(0)] private Vector2Int fragmentChipDropRange;
    [SerializeField] private Chips fragmentChipPrefab;
    
    [SerializeField] private float groundDistance = 1f;
    [SerializeField] private LayerMask groundLayer;

    protected override void Awake()
    {
        base.Awake();
        
        healthComponent.OnDeath.AddListener(DropFragmentChip);
    }

    private void DropFragmentChip()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, groundDistance, groundLayer))
        {
            Chips fragmentChip = Instantiate(fragmentChipPrefab, hitInfo.point, Quaternion.identity);
            fragmentChip.SetChipsAmount((uint)Random.Range(fragmentChipDropRange.x, fragmentChipDropRange.y));
        }
    }
}
