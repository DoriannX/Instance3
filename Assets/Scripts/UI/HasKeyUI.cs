using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class HasKeyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyStateText;

    private void Awake()
    {
        Assert.IsNotNull(keyStateText);
    }

    private void Start()
    {
        keyStateText.text = Player.hasKey.ToString();
        Player.onKeyStateChanged += PlayerOnonKeyStateChanged;
    }

    private void PlayerOnonKeyStateChanged()
    {
        keyStateText.text = Player.hasKey.ToString();
    }

    private void OnDestroy()
    {
        Player.onKeyStateChanged -= PlayerOnonKeyStateChanged;
    }
}
