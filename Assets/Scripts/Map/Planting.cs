using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Planting : MonoBehaviour
{
    /// <summary>
    /// 1. 나무를 심을 청크를 몇개 고른다 (청크리스트중에서 랜덤)
    /// 2. 청크 내에서 나무를 랜덤으로 심는다
    ///     2-1. 하늘에서 raycast해서 Grond를 검사한다
    ///     2-2. 그 위치에 나무 설치 
    ///     2-3. 만약 물이면 패스 
    ///     2-4. 너무 높으면 패스
    /// </summary>

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask oceanLayer;
    [SerializeField] private int groundlayerInt;

    [SerializeField] private int chunchMin;
    [SerializeField] private int chunchMax;
    [SerializeField] private float maxGroundHeight;
    [SerializeField] private List<GameObject> plantList;

    [SerializeField] private MapManager mapManager;

    private void Start()
    {
        mapManager = GetComponent<MapManager>();

        groundLayer = LayerMask.GetMask("Ground");
        oceanLayer = LayerMask.GetMask("Ocean");
        groundlayerInt = LayerMask.NameToLayer("Ground");

        chunchMin = 10;
        chunchMax = 10;
        maxGroundHeight = 50f;
    }

    public void PlantingTree(List<Transform> chuck , List<GameObject> plant) 
    {
        // 청크는 항상 2 ~ 4 개 정도 사이에서 랜덤
        int ranchunck = Random.Range(2, chuck.Count);

        // 임시로 담아놓기
        this.plantList = plant;

        // 랜덤 수 구하기 
        List<int> suffleChunk = Shuffle(2, 0 , chuck.Count - 1);

        foreach (int num in suffleChunk) 
        {
            Transform trs = chuck[num];

            // 청크 기준으로 ( x - 5, y - 5 ) ~ ( x + 5 , y + 5) 사이에서 랜덤으로 N개 고르기
            List<int> selectPlantX = Shuffle(5 , (int)trs.position.x - chunchMin, (int)trs.position.x + chunchMax);
            List<int> selectPlantZ = Shuffle(5 , (int)trs.position.z - chunchMin, (int)trs.position.z + chunchMax);

            for (int i = 0; i < selectPlantX.Count; i++) 
            {
                RaycastToGround(selectPlantX[i] , selectPlantZ[i]);
            }
        }

    }

    private void RaycastToGround(float x, float z) 
    {
        // 기준 위치에서 Nf 정도 올려서, 바닥쪽으로 raycast
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(x, 50f, z) , Vector3.down);

        // Instantiate(plantList[0], new Vector3(x, 100f, z) , Quaternion.identity);

        // ground 레이어만 검사 
        if (Physics.Raycast(ray, out hit, 100f, groundLayer)) 
        {
            // 너무 높으면 패스
            if (hit.transform.position.y >= maxGroundHeight)
                return;

            // 이 위치에 나무 심기 
            PlantTreeOrEct(hit.point);
        }
    }

    private void PlantTreeOrEct(Vector3 position) 
    {
        int rand = Random.Range(0, plantList.Count);

        GameObject tree = Instantiate( plantList[rand] );
        tree.transform.position = position;
        tree.transform.SetParent(mapManager.TreeParent);
    }

    // min과 max 사이에서 selectNum 갯수만큼 중복없이 랜덤수 리턴
    private List<int> Shuffle(int selectNum , int minNum, int maxNum) 
    {
        // 선택할 인텍스 저장 
        List<int> selectedIndex = new List<int>();

        // 임시 리스트
        List<int> temp = new List<int>();
        for (int i = minNum; i <= maxNum; i++) 
        {
            temp.Add(i);
        }

        // 랜덤하게 3개 선택 
        for(int i = 0; i < selectNum; i++) 
        {
            // 남은 인덱스 중에서 하나 고르기
            int ran = Random.Range(0, temp.Count);

            // 선택 인덱스 저장 
            selectedIndex.Add(temp[ran]);

            // 선택된거 지우기 
            temp.RemoveAt(ran);

        }

        return selectedIndex;
    }
}
