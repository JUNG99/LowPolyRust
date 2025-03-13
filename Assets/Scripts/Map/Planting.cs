using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Planting : MonoBehaviour
{
    /// <summary>
    /// 1. ������ ���� ûũ�� � ���� (ûũ����Ʈ�߿��� ����)
    /// 2. ûũ ������ ������ �������� �ɴ´�
    ///     2-1. �ϴÿ��� raycast�ؼ� Grond�� �˻��Ѵ�
    ///     2-2. �� ��ġ�� ���� ��ġ 
    ///     2-3. ���� ���̸� �н� 
    ///     2-4. �ʹ� ������ �н�
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

        // �ӽ÷� ��Ƴ���
        this.mapData = mapdata;
        this.plantPrefabList = plantPrefab;
        this.plantingParent = parentTrs;

        // ûũ���� �� N��
        int selectChuckCount = mapdata.SelectChunkCount;       

        // ��ü ûũ���� N�� ���� 
        List<int> suffleChunk = Shuffle(selectChuckCount, 0 , chuck.Length - 1);

        foreach (int num in suffleChunk) 
        {
            Transform trs = chuck[num];

            // �������� N�� ����
            int plantCount = Random.Range(mapdata.PlantCountMin , mapdata.PlantCountMax + 1);

            // ûũ ��������(x -size, y - size ) ~(x + size, y + size) ������ �� 
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
        // ���� ��ġ���� Nf ���� �÷���, �ٴ������� raycast
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(x, 50f, z) , Vector3.down);


        // ground ���̾ �˻� 
        if (Physics.Raycast(ray, out hit, 100f, groundLayer)) 
        {
            // �ʹ� ������ �н�
            if (hit.transform.position.y >= maxGroundHeight)
                return;

            // �� ��ġ�� ���� �ɱ� 
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

    // min�� max ���̿��� selectNum ������ŭ �ߺ����� ������ ����
    private List<int> Shuffle(int selectNum , int minNum, int maxNum) 
    {
        // ������ ���ؽ� ���� 
        List<int> selectedIndex = new List<int>();

        // �ӽ� ����Ʈ
        List<int> temp = new List<int>();
        for (int i = minNum; i <= maxNum; i++) 
        {
            temp.Add(i);
        }

        // �����ϰ� 3�� ���� 
        for(int i = 0; i < selectNum; i++) 
        {
            // ���� �ε��� �߿��� �ϳ� ����
            int ran = Random.Range(0, temp.Count);

            // ���� �ε��� ���� 
            selectedIndex.Add(temp[ran]);

            // ���õȰ� ����� 
            temp.RemoveAt(ran);

        }

        return selectedIndex;
    }
}
