using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Image hpBar;
    public Image hungerBar;
    public Image thirstBar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHP(float currentHP, float maxHP)
    {
        if (hpBar) hpBar.fillAmount = currentHP / maxHP;
    }

    public void UpdateHunger(float currentHunger, float maxHunger)
    {
        if (hungerBar) hungerBar.fillAmount = currentHunger / maxHunger;
    }

    public void UpdateThirst(float currentThirst, float maxThirst)
    {
        if (thirstBar) thirstBar.fillAmount = currentThirst / maxThirst;
    }
}
