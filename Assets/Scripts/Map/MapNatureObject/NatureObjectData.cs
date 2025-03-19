using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NatureObjectData", menuName = "MapData/CreateNatureObjectData")]
public class NatureObjectData : ScriptableObject
{
    [SerializeField]
    private PlantType plantType;
    [SerializeField]
    private string natureName;
    [SerializeField]
    private string natureToolTip;
    [SerializeField]
    private int durability;
    [SerializeField]
    private GameObject itemPrefab;

    public PlantType PlantType { get => plantType;}
    public int Durability { get => durability;}
    public string NatureName { get => natureName; }
    public string NatureToolTip { get => natureToolTip; }
    public GameObject ItemPrefab { get => itemPrefab; }
}
