using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "MapData/CreateMapData")]

public class MapData : ScriptableObject
{
    [SerializeField] 
    private PlantType plantType;
    [SerializeField]
    private float generateSecond;       // ���� ��Ÿ��
    [SerializeField]
    private int selectChunkCount;       // ûũ�߿� �� ����
    [SerializeField]
    private int plantCountMin;          // ���� �ּ� ����
    [SerializeField]
    private int plantCountMax;          // ���� �ִ� ����
    [SerializeField]
    private int chunkSizeMin;           // ûũ �ּ� ������
    [SerializeField]
    private int chunkSizeMax;           // ûũ �ִ� ������

    public PlantType PlantType { get => plantType;  }
    public float GenerateSecond { get => generateSecond; }
    public int SelectChunkCount { get => selectChunkCount;  }
    public int PlantCountMin { get => plantCountMin; }
    public int PlantCountMax { get => plantCountMax; }
    public int ChunkSizeMin { get => chunkSizeMin;  }
    public int ChunkSizeMax { get => chunkSizeMax;  }
}
