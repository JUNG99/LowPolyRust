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

    // 플레이어가 Raycast해서 GetComponent로 사용할 함수
    public void HarvesstNatureObject()
    {
        // 나무,풀,돌 내구도 한개 줄이기
        durability -= 1;

        // 아이템 드랍 하는 방식으로 변경 
        Instantiate(naturalObjectData.ItemPrefab, transform.position + new Vector3(0,1f,0), Quaternion.identity);

        // duration (내구도)를 다 쓰면 
        if(durability <= 0)
        {
            // 오브젝트 삭제 
            // 오브젝트 부모
            //      ㄴ 오브젝트 콜라이더 (raycast 되는 부분 )
            Destroy(gameObject.transform.parent.gameObject, 0.5f);
        }
    }

}
