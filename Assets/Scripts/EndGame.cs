using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject endMenuUI;
    [SerializeField] private LayerMask playerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if(((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            EndMenu();
        }
    }

    public void EndMenu()
    {
        endMenuUI.SetActive(true);
    }
}
