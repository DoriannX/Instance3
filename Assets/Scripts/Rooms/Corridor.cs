using UnityEngine;
using UnityEngine.Assertions;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class Corridor : MonoBehaviour
{
    //corridor's entrance
    [Header("CorridorLength")]
    [SerializeField] private GameObject corridorStart; 
    [SerializeField] private GameObject corridorEnd;

    //corridor's doorframe and hall
    [Header("ChieldReference")]
    [SerializeField] private GameObject corridorLeft;
    [SerializeField] private GameObject corridorMiddle;
    [SerializeField] private GameObject corridorRight;

    //if the corridor is big or small ( true = big )
    [Header("CorridorSize")]
    [SerializeField] private bool corridorIsBig;
    [SerializeField] private Vector3 corridorSizeBig = new(0.66f,0.66f,0.66f);
    [SerializeField] private Vector3 corridorSizeSmall = new(0.5f, 0.5f, 0.5f);

    public void Awake()
    {
        Assert.IsNotNull(corridorStart);
        Assert.IsNotNull(corridorEnd);
        Assert.IsNotNull(corridorLeft);
        Assert.IsNotNull(corridorMiddle);
        Assert.IsNotNull(corridorRight);
    }

    public void CorridorTransform(Transform startPos, Transform endPos)
    {

        corridorMiddle.transform.localScale = new Vector3(1, 1, Vector3.Distance(endPos.position,startPos.position) * 1/transform.localScale.z);
        corridorMiddle.transform.position = new Vector3((startPos.transform.position.x + endPos.transform.position.x)/2, (startPos.transform.position.y + endPos.transform.position.y) / 2, (startPos.transform.position.z + endPos.transform.position.z) / 2);

        
        var dir = endPos.transform.position - startPos.transform.position;
        var rot = Quaternion.LookRotation(dir, Vector3.up);

        corridorLeft.transform.rotation = rot;
        corridorMiddle.transform.rotation = rot;
        corridorRight.transform.rotation = rot;
        
    }

    private void Update()
    {
        if (corridorIsBig)
        {
            transform.localScale = corridorSizeBig;
        }
        else
        {
            transform.localScale = corridorSizeSmall;
        }

        corridorRight.transform.position = corridorStart.transform.position;
        corridorLeft.transform.position = corridorEnd.transform.position;
        corridorLeft.transform.position = new Vector3(corridorLeft.transform.position.x, corridorRight.transform.position.y, corridorLeft.transform.position.z);
        CorridorTransform(corridorLeft.transform, corridorRight.transform);
    }
}
