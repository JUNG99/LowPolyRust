using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlantType
{
    Tree,
    Rock,
    Grass
}

public class MapManager : MonoBehaviour
{
    [SerializeField] private Planting planting;

    [Header("=== Parent ===")]
    [SerializeField] private Transform treeParent;
    [SerializeField] private Transform rockParent;
    [SerializeField] private Transform grassParent;

    [Header("===Chunck===")]
    [SerializeField] Transform treeChunkParent;
    [SerializeField] Transform rockChunkParent;
    [SerializeField] Transform grassChunkParent;
    [SerializeField] Transform[] treeChunks;
    [SerializeField] Transform[] rockChunks;
    [SerializeField] Transform[] grassChunks;

    [Header("===Prefab===")]            // ##TODO : Pooling���� ������
    [SerializeField] List<GameObject> treePrefab;
    [SerializeField] List<GameObject> rockPrefab;
    [SerializeField] List<GameObject> grassPrefab;

    [Header("===Data===")]
    [SerializeField] List<MapData> mapData;
    // PlantType ������� ������ 

    public Transform TreeParent { get => treeParent;}

    void Start()
    {
        planting = GetComponent<Planting>();

        InitTrsnform();
        InitGeneratePlant();

        StartCoroutine(TreePlant());
        StartCoroutine(RockPlant());
        StartCoroutine(GrassPlant());
    }

    private void InitTrsnform() 
    {
        // GetComponentsInChildren : �θ���� ���Խ�Ŵ 
        // �θ��� �������� �ڽĸ� ��������
        treeChunks = new Transform[treeChunkParent.childCount];
        rockChunks = new Transform[rockChunkParent.childCount];
        grassChunks = new Transform[grassChunkParent.childCount];

        for (int i = 0; i < treeChunkParent.childCount; i++)
        {
            treeChunks[i] = treeChunkParent.GetChild(i);
        }

        for (int i = 0; i < rockChunkParent.childCount; i++)
        {
            rockChunks[i] = rockChunkParent.GetChild(i);
        }

        for (int i = 0; i < grassChunkParent.childCount; i++)
        {
            grassChunks[i] = grassChunkParent.GetChild(i);
        }
    }

    private void InitGeneratePlant() 
    {
        // �� ó������ ������ �����ϴ°͵� ������
        // ���� ����
        planting.PlantingTree(mapData[(int)PlantType.Tree], treeParent, treeChunks, treePrefab);
        // �� ����
        planting.PlantingTree(mapData[(int)PlantType.Rock], rockParent, rockChunks, rockPrefab);
        // Ǯ ����
        planting.PlantingTree(mapData[(int)PlantType.Grass], grassParent, grassChunks, grassPrefab);
        planting.PlantingTree(mapData[(int)PlantType.Grass], grassParent, grassChunks, grassPrefab);
    }

    private IEnumerator TreePlant() 
    {
        while (true) 
        {
            // ���� ����
            planting.PlantingTree(mapData[(int)PlantType.Tree] , treeParent , treeChunks, treePrefab);

            // ��� 
            yield return new WaitForSeconds(mapData[(int)PlantType.Tree].GenerateSecond);
        }
    }

    private IEnumerator RockPlant() 
    {
        while (true)
        {
            // �� ����
            planting.PlantingTree(mapData[(int)PlantType.Rock], rockParent, rockChunks, rockPrefab);

            // ��� 
            yield return new WaitForSeconds(mapData[(int)PlantType.Rock].GenerateSecond);
        }
    }

    private IEnumerator GrassPlant() 
    {
        while (true)
        {
            // Ǯ ����
            planting.PlantingTree(mapData[(int)PlantType.Grass], grassParent, grassChunks, grassPrefab);

            // ��� 
            yield return new WaitForSeconds(mapData[(int)PlantType.Grass].GenerateSecond);
        }
    }


}
