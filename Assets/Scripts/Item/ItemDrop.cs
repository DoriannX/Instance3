using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))] 
[RequireComponent(typeof(MeshRenderer))] 
[RequireComponent(typeof(Mesh))] 

// put one of the child classes on a gameobject, specify the amount it is given

abstract public class ItemDrop : MonoBehaviour //Assign the "Drop" tag to the gameobject with the trigger that will detect the collision
{
    abstract public bool GetPicked();
    abstract public void OnPickUp(GameObject gm);
    abstract protected IEnumerator GoToEntity();
}
