using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class HasKeyUI : MonoBehaviour
{
    [SerializeField] private Image keyImage;
    [SerializeField] private Sprite HasState, HasNotState;

    private void Awake()
    {
        Assert.IsNotNull(keyImage);
    }

    private void Start()
    {
        //keyStateText.text = Player.hasKey.ToString();
        keyImage.sprite = Player.hasKey ? HasState : HasNotState;
        Player.onKeyStateChanged += PlayerOnonKeyStateChanged;
    }

    private void PlayerOnonKeyStateChanged()
    {
        keyImage.sprite = Player.hasKey ? HasState : HasNotState;
    }

    private void OnDestroy()
    {
        Player.onKeyStateChanged -= PlayerOnonKeyStateChanged;
    }
}
