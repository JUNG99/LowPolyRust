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
            // ���� ����
            planting.PlantingTree(chuck, treePrefab);

            // ��� 
            yield return new WaitForSeconds(generateTime);
        }
    }
    
}
