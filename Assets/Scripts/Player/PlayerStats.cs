using UnityEngine;

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

    private void Start()
    {
        currentHP = maxHP;
        currentHunger = maxHunger;
        currentThirst = maxThirst;

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

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die(); // ü���� 0 ���ϰ� �Ǹ� ���� ó��
        }
        UpdateUI(); // ü�� UI ������Ʈ
    }

    private void Die()
    {
        // �׾��� �� ó��
        Debug.Log("Player died");
    }
    private void UpdateUI()
    {
        UIManager.Instance.UpdateHP(currentHP, maxHP);
        UIManager.Instance.UpdateHunger(currentHunger, maxHunger);
        UIManager.Instance.UpdateThirst(currentThirst, maxThirst);
    }
}
