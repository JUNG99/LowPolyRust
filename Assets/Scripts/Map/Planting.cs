using System;
using System.Collections.Generic;
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
        // ûũ�� �׻� 2 ~ 4 �� ���� ���̿��� ����
        int ranchunck = Random.Range(2, chuck.Count);

        // �ӽ÷� ��Ƴ���
        this.plantList = plant;

        // ���� �� ���ϱ� 
        List<int> suffleChunk = Shuffle(2, 0 , chuck.Count - 1);

        foreach (int num in suffleChunk) 
        {
            Transform trs = chuck[num];

            // ûũ �������� ( x - 5, y - 5 ) ~ ( x + 5 , y + 5) ���̿��� �������� N�� ����
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
        // ���� ��ġ���� Nf ���� �÷���, �ٴ������� raycast
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(x, 50f, z) , Vector3.down);

        // Instantiate(plantList[0], new Vector3(x, 100f, z) , Quaternion.identity);

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
        int rand = Random.Range(0, plantList.Count);

        GameObject tree = Instantiate( plantList[rand] );
        tree.transform.position = position;
        tree.transform.SetParent(mapManager.TreeParent);
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
