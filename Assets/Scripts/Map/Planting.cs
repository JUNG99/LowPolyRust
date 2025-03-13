using System;
using System.Collections.Generic;
using System.Drawing;
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

    [SerializeField] private float maxGroundHeight;
    [SerializeField] private List<GameObject> plantPrefabList;

    [Header("===currData===")]
    [SerializeField] MapData mapData;
    [SerializeField] Transform plantingParent;

    private void Start()
    {
        maxGroundHeight = 60f;
        groundLayer = LayerMask.GetMask("Ground");
    }

    public void PlantingTree(MapData mapdata, Transform parentTrs, Transform[] chuck , List<GameObject> plantPrefab ) 
    {

        // 임시로 담아놓기
        this.mapData = mapdata;
        this.plantPrefabList = plantPrefab;
        this.plantingParent = parentTrs;

        // 청크에서 고를 N개
        int selectChuckCount = mapdata.SelectChunkCount;       

        // 전체 청크에서 N개 고르기 
        List<int> suffleChunk = Shuffle(selectChuckCount, 0 , chuck.Length - 1);

        foreach (int num in suffleChunk) 
        {
            Transform trs = chuck[num];

            // 랜덤으로 N개 고르기
            int plantCount = Random.Range(mapdata.PlantCountMin , mapdata.PlantCountMax + 1);

            // 청크 기준으로(x -size, y - size ) ~(x + size, y + size) 사이의 값 
            List<int> selectPlantX = Shuffle(plantCount, (int)trs.position.x - mapdata.ChunkSizeMin, (int)trs.position.x + mapdata.ChunkSizeMax);
            List<int> selectPlantZ = Shuffle(plantCount, (int)trs.position.z - mapdata.ChunkSizeMin, (int)trs.position.z + mapdata.ChunkSizeMax);

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
        int rand = Random.Range(0, plantPrefabList.Count);

        GameObject tree = Instantiate( plantPrefabList[rand] );
        tree.transform.position = position;
        tree.transform.SetParent(plantingParent);
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
