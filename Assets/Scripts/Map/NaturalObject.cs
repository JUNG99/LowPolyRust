using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalObject : MonoBehaviour
{
    [Header("=== Data ===")]
    [SerializeField]
    private NatureObjectData naturalObjectData;

    // �÷��̾ Raycast�ؼ� GetComponent�� ����� �Լ�
    public void HarvesstNatureObject()
    {
        // ����,Ǯ,�� ������ �Ѱ� ���̱�
        if (naturalObjectData.UseDurability(1))
        {
            // ##TODO : ������ ȹ�� ���� �ۼ� �ʿ�
            
            return;
        }
        // duration (������)�� �� ���� 
        else 
        {
            // ������Ʈ ���� 
            // ##TODO : pooling�� ����ֱ�
            Destroy(gameObject, 0.5f);
        }
    }

}
