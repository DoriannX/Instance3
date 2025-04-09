using System.Collections;
using UnityEngine;

public class Bandages : ItemDrop
{
    [SerializeField] private uint healAmnt;
    [SerializeField] private GameObject entity;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private float travelTime;
    [SerializeField] private bool gotPickedUp = false;
    [SerializeField] private bool arrived = false;

    public override void OnPickUp(GameObject gm) //Get the actor that called the 
    {
        entity = gm;
        startPos = transform.position;
        if (!gotPickedUp) StartCoroutine(GoToEntity());
        gotPickedUp = true;
    }

    protected override IEnumerator GoToEntity()
    {
        float elapsedTime = 0;
        while (elapsedTime < travelTime)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, entity.transform.position, elapsedTime / travelTime);
            if (arrived)
            {
                StopAllCoroutines();
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                Destroy(gameObject);
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == entity && !other.isTrigger) //when the object collides with the one that called him he will start its own destruction
        {
            arrived = true;
        }
    }

    public override bool GetPicked()
    {
        return gotPickedUp;
    }

    private void OnDestroy()
    {
        PlayerScript playerScript;
        if (entity.TryGetComponent<PlayerScript>(out playerScript)) // If the script that called is the PlayerScript, EntityHealth will be assigned thanks to RequiredComponent
        {
            entity.GetComponent<EntityHealth>().Heal((int)healAmnt);
        }
        else
        {
            Debug.Log($"Wrong Entity : {entity}");
        }
    }
}
