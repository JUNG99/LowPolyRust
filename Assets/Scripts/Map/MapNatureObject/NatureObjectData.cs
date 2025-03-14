using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NatureObjectData", menuName = "MapData/CreateNatureObjectData")]
public class NatureObjectData : ScriptableObject
{
    [SerializeField]
    private PlantType plantType;
    [SerializeField]
    private int durability;

    // 아이템 획득 관련해서는 추후 추가 

    public PlantType PlantType { get => plantType;}
    public int Durability { get => durability;}

    public bool UseDurability(int amout) 
    {
        durability -= amout;

        if (durability > 0)
        { 
            return true;
        }    
        else
            return false;
        
    }

}
