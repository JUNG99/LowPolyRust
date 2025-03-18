using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalObject : MonoBehaviour
{
    [Header("=== Data ===")]
    [SerializeField]
    private NatureObjectData naturalObjectData;
    [SerializeField]
    private int durability;

    public NatureObjectData NaturalObjectData { get => naturalObjectData; }

    private void Start()
    {
        durability = naturalObjectData.Durability;
    }

    // �÷��̾ Raycast�ؼ� GetComponent�� ����� �Լ�
    public void HarvesstNatureObject()
    {
        // ����,Ǯ,�� ������ �Ѱ� ���̱�
        durability -= 1;

        // ������ ��� �ϴ� ������� ���� 
        Instantiate(naturalObjectData.ItemPrefab, transform.position, Quaternion.identity);

        // duration (������)�� �� ���� 
        if(durability <= 0)
        {
            // ������Ʈ ���� 
            // ������Ʈ �θ�
            //      �� ������Ʈ �ݶ��̴� (raycast �Ǵ� �κ� )
            Destroy(gameObject.transform.parent.gameObject, 0.5f);
        }
    }

}
