using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalObject : MonoBehaviour
{
    [Header("=== Data ===")]
    [SerializeField]
    private NatureObjectData naturalObjectData;

    // 플레이어가 Raycast해서 GetComponent로 사용할 함수
    public void HarvesstNatureObject()
    {
        // 나무,풀,돌 내구도 한개 줄이기
        if (naturalObjectData.UseDurability(1))
        {
            // ##TODO : 아이템 획득 로직 작성 필요
            
            return;
        }
        // duration (내구도)를 다 쓰면 
        else 
        {
            // 오브젝트 삭제 
            // ##TODO : pooling에 집어넣기
            Destroy(gameObject, 0.5f);
        }
    }

}
