using Item.Drops;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Fragment Chip Drop")] 
    [SerializeField] private uint fragmentChipDropRangeMin;
    [SerializeField] private uint fragmentChipDropRangeMax;
    [SerializeField] private Chips fragmentChipPrefab;
    
    [SerializeField] private float groundDistance = 1f;
    [SerializeField] private LayerMask groundLayer;
    
    private void Start()
    {
        healthComponent.onDeath += DropFragmentChip;
    }
    
    private void DropFragmentChip()
    {
        Debug.Log(" Dropping fragment chip...");
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, groundDistance, groundLayer))
        {
            Debug.Log("Dropping fragment chip at: " + hitInfo.point);
            Chips fragmentChip = Instantiate(fragmentChipPrefab, hitInfo.point, Quaternion.identity);
            fragmentChip.SetChipsAmount((uint)Random.Range(fragmentChipDropRangeMin, fragmentChipDropRangeMax));
        }
        Destroy(gameObject);
    }
}
