using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("References")]
    private EntityHealth entityHealth;
    [SerializeField] private Image easeHealthBar; //Start color hex = 3A5B2B // Low Health color hex = 7E271E
    [SerializeField] private Image healthBar; //Start color hex = green // Low Health color hex = red

    [Header("Settings")]
    [SerializeField] private float easeLerpSpeed = 4f;
    [SerializeField] private float colorLerpSpeed = 5f;
    private bool isLerping = false;
    private bool isColorLerping = false;
    private bool isAlreadylowHealth = false;

    private void Awake()
    {
        entityHealth = GetComponent<EntityHealth>();
               
        Assert.IsNotNull(easeHealthBar, "Ease Health Bar is not assigned in the inspector.");                
        Assert.IsNotNull(healthBar, "Health Bar is not assigned in the inspector.");        
    }

    private void Start()
    {
        entityHealth.onHealthChanged += UpdateHealthBar;
        healthBar.fillAmount = (float)entityHealth.Hp/entityHealth.maxHp;
        easeHealthBar.fillAmount = (float)entityHealth.Hp/entityHealth.maxHp;
    }

    private void Update()
    {
        if (isLerping)
        {
            easeHealthBar.fillAmount = Mathf.Lerp(easeHealthBar.fillAmount, (float)entityHealth.Hp/entityHealth.maxHp, easeLerpSpeed * Time.deltaTime);         

            if (easeHealthBar.fillAmount - (float)(entityHealth.Hp / entityHealth.maxHp) <= 0.1)
            {
                easeHealthBar.gameObject.SetActive(false);
                isLerping = false;
            }
        }

        if(isColorLerping)
        {
            ColorTransition(healthBar, Color.red);
          
            Color color = new Color32(0x7E, 0x27, 0x1E, 0xFF);               
            ColorTransition(easeHealthBar, color);

            if (healthBar.color == Color.red)
            {
                isColorLerping = false;
            }
        }
    }

    public void UpdateHealthBar(int previousLife, int newLife)
    {
        if (!healthBar.gameObject.activeSelf)
            healthBar.gameObject.SetActive(true);

        if (!easeHealthBar.gameObject.activeSelf)
            easeHealthBar.gameObject.SetActive(true);

        healthBar.fillAmount = (float)entityHealth.Hp / entityHealth.maxHp;

        isLerping = true;

        if(!isAlreadylowHealth && entityHealth.Hp <= entityHealth.maxHp * 0.5f)
        {
            isColorLerping = true;
            isAlreadylowHealth = true;
        }
    }

    private void ColorTransition(Image bar, Color color)
    {
        bar.color = Color.Lerp(bar.color, color, colorLerpSpeed * Time.deltaTime);
    }
}
