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
        durability -= 1;

        // ����,Ǯ,�� ������ �Ѱ� ���̱�
        if (durability > 0)
        {
            // ##TODO : ������ ȹ�� ���� �ۼ� �ʿ�
            
            return;
        }
        // duration (������)�� �� ���� 
        else 
        {
            // ������Ʈ ���� 
            // ������Ʈ �θ�
            //      �� ������Ʈ �ݶ��̴� (raycast �Ǵ� �κ� )
            Destroy(gameObject.transform.parent.gameObject, 0.5f);
        }
    }

}
