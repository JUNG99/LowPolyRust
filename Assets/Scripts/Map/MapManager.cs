using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Planting planting;

    [Header("=== Chunck / Tree ===")]
    [SerializeField] private Transform treeParent;
    [SerializeField] List<Transform> chuck;
    [SerializeField] List<GameObject> treePrefab;

    [Header("===Cycle===")]
    [SerializeField] float generateTime;

    public Transform TreeParent { get => treeParent;}

    void Start()
    {
        planting = GetComponent<Planting>();

        StartCoroutine(TreePlant());
    }

    private IEnumerator TreePlant() 
    {
        while (true) 
        {
            // 나무 생성
            planting.PlantingTree(chuck, treePrefab);

            // 대기 
            yield return new WaitForSeconds(generateTime);
        }
    }
    
}
