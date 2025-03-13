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

    [Header("===Prefab===")]            // ##TODO : Pooling으로 빼야함
    [SerializeField] List<GameObject> treePrefab;
    [SerializeField] List<GameObject> rockPrefab;
    [SerializeField] List<GameObject> grassPrefab;

    [Header("===Data===")]
    [SerializeField] List<MapData> mapData;
    // PlantType 순서대로 들어가야함 

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
        // GetComponentsInChildren : 부모까지 포함시킴 
        // 부모의 직접적인 자식만 가져오기
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
        // 맨 처음에는 여러번 생성하는것도 좋을듯
        // 나무 생성
        planting.PlantingTree(mapData[(int)PlantType.Tree], treeParent, treeChunks, treePrefab);
        // 돌 생성
        planting.PlantingTree(mapData[(int)PlantType.Rock], rockParent, rockChunks, rockPrefab);
        // 풀 생성
        planting.PlantingTree(mapData[(int)PlantType.Grass], grassParent, grassChunks, grassPrefab);
        planting.PlantingTree(mapData[(int)PlantType.Grass], grassParent, grassChunks, grassPrefab);
    }

    private IEnumerator TreePlant() 
    {
        while (true) 
        {
            // 나무 생성
            planting.PlantingTree(mapData[(int)PlantType.Tree] , treeParent , treeChunks, treePrefab);

            // 대기 
            yield return new WaitForSeconds(mapData[(int)PlantType.Tree].GenerateSecond);
        }
    }

    private IEnumerator RockPlant() 
    {
        while (true)
        {
            // 돌 생성
            planting.PlantingTree(mapData[(int)PlantType.Rock], rockParent, rockChunks, rockPrefab);

            // 대기 
            yield return new WaitForSeconds(mapData[(int)PlantType.Rock].GenerateSecond);
        }
    }

    private IEnumerator GrassPlant() 
    {
        while (true)
        {
            // 풀 생성
            planting.PlantingTree(mapData[(int)PlantType.Grass], grassParent, grassChunks, grassPrefab);

            // 대기 
            yield return new WaitForSeconds(mapData[(int)PlantType.Grass].GenerateSecond);
        }
    }


}
