using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float maxHP = 100f;
    public float maxHunger = 100f;
    public float maxThirst = 100f;

    private float currentHP;
    private float currentHunger;
    private float currentThirst;

    public float hungerDecreaseRate = 1f;
    public float thirstDecreaseRate = 1.5f;
    public float hpDecreaseRate = 5f;

    //UI ��� �߰�
    public Slider hpBar;
    public Slider hungerBar;
    public Slider thirstBar;

    private void Start()
    {
        currentHP = maxHP;
        currentHunger = maxHunger;
        currentThirst = maxThirst;

        //UI�ʱ�ȭ
        UpdateUI();

        InvokeRepeating(nameof(DecreaseStatsOverTime), 1f, 1f);
    }

    private void DecreaseStatsOverTime()
    {
        if (currentHunger > 0)
        {
            currentHunger -= hungerDecreaseRate;
        }
        else
        {
            if (currentThirst > 0)
            {
                currentThirst -= thirstDecreaseRate;
            }
        }

        if (currentThirst <= 0 && currentHP > 0)
        {
            currentHP -= hpDecreaseRate;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (hpBar) hpBar.value = currentHP;
        if (hungerBar) hungerBar.value = currentHunger;
        if (thirstBar) thirstBar.value = currentThirst;
    }
}
