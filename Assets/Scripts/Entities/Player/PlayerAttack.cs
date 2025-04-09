using Pooling;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Weapons References")]
    private MeleeWeapon meleeWeapon;
    private RangeWeapon rangeWeapon;
    private Transform playerTransform;
    private PoolSpawner poolSpawner;

    [Header("Weapons Settings")]
    [SerializeField] private LayerMask enemyLayer;
    private float meleeCooldownTimer = 0f;
    private float rangeCooldownTimer = 0f;

    private void Awake()
    {
        meleeWeapon = GetComponentInChildren<MeleeWeapon>(true);
        rangeWeapon = GetComponentInChildren<RangeWeapon>(true);
        playerTransform = GetComponent<Transform>();
        poolSpawner = GetComponentInChildren<PoolSpawner>(true);
    }    

    private void Update()
    {
        if (meleeCooldownTimer > 0f)
        {
            meleeCooldownTimer -= Time.deltaTime;
        }

        if (rangeCooldownTimer > 0f)
        {
            rangeCooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (meleeWeapon.gameObject.activeSelf && meleeCooldownTimer <= 0f)
        {
            MeleeAttack();
            meleeCooldownTimer = meleeWeapon.cooldown;
        }

        if (rangeWeapon.gameObject.activeSelf && rangeCooldownTimer <= 0f)
        {
            RangeAttack();
            rangeCooldownTimer = rangeWeapon.cooldown;
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
        if(rangeWeapon.ammoAmount > 0)
        {
            Transform bulletSpawner = poolSpawner.transform;

            Bullet bullet = poolSpawner.pool.Get();
            bullet.transform.position = bulletSpawner.position;
            bullet.transform.rotation = playerTransform.rotation;
            bullet.gameObject.SetActive(true);
            rangeWeapon.ammoAmount--;
        }
        else
        {
            Debug.Log("No ammo left");
            return;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    // Dessiner la bo�te du BoxCast      
    //    Vector3 boxHalfExtents = Vector3.one * meleeWeapon.attackRange; // Taille de la bo�te (moiti� des dimensions)
    //    Quaternion orientation = playerTransform.rotation; // Orientation de la bo�te

    //    // Couleur de la bo�te
    //    Gizmos.color = Color.red;        

    //    // Dessiner la bo�te � la position finale
    //    Gizmos.matrix = Matrix4x4.TRS(playerTransform.position + playerTransform.forward * meleeWeapon.attackRange, orientation, Vector3.one);
    //    Gizmos.DrawCube(Vector3.zero, boxHalfExtents*2);
    //}
}
