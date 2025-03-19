using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "MapData/CreateMapData")]

public class MapData : ScriptableObject
{
    [SerializeField] 
    private PlantType plantType;
    [SerializeField]
    private float generateSecond;       // 생성 쿨타임
    [SerializeField]
    private int selectChunkCount;       // 청크중에 고를 갯수
    [SerializeField]
    private int plantCountMin;          // 심을 최소 갯수
    [SerializeField]
    private int plantCountMax;          // 심을 최대 갯수
    [SerializeField]
    private int chunkSizeMin;           // 청크 최소 사이즈
    [SerializeField]
    private int chunkSizeMax;           // 청크 최대 사이즈

    public PlantType PlantType { get => plantType;  }
    public float GenerateSecond { get => generateSecond; }
    public int SelectChunkCount { get => selectChunkCount;  }
    public int PlantCountMin { get => plantCountMin; }
    public int PlantCountMax { get => plantCountMax; }
    public int ChunkSizeMin { get => chunkSizeMin;  }
    public int ChunkSizeMax { get => chunkSizeMax;  }
}
