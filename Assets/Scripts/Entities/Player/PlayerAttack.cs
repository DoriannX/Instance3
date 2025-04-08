using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    private MeleeWeapon meleeWeapon;
    private RangeWeapon rangeWeapon;
    private Transform playerTransform;

    private void Start()
    {
        meleeWeapon = GetComponentInChildren<MeleeWeapon>(true);
        rangeWeapon = GetComponentInChildren<RangeWeapon>(true);
        playerTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }        
    }

    private void Attack()
    {
        if(meleeWeapon.gameObject.activeSelf)
        {
            MeleeAttack();
        }

        if (rangeWeapon.gameObject.activeSelf)
        {
            RangeAttack();
        }
    }

    private void MeleeAttack()
    {
        Collider[] hitColliders = Physics.OverlapBox(playerTransform.position + playerTransform.forward * meleeWeapon.attackRange, (Vector3.one * meleeWeapon.attackRange), playerTransform.rotation, enemyLayer);

        if(hitColliders.Length > 0)
        {
            Debug.Log("Hit enemies");
        }
    }

    private void RangeAttack()
    {
        Debug.Log("Range attack");
    }

    //private void OnDrawGizmos()
    //{
    //    // Dessiner la boîte du BoxCast      
    //    Vector3 boxHalfExtents = Vector3.one * meleeWeapon.attackRange; // Taille de la boîte (moitié des dimensions)
    //    Quaternion orientation = playerTransform.rotation; // Orientation de la boîte

    //    // Couleur de la boîte
    //    Gizmos.color = Color.red;        

    //    // Dessiner la boîte à la position finale
    //    Gizmos.matrix = Matrix4x4.TRS(playerTransform.position + playerTransform.forward * meleeWeapon.attackRange, orientation, Vector3.one);
    //    Gizmos.DrawCube(Vector3.zero, boxHalfExtents*2);
    //}
}
