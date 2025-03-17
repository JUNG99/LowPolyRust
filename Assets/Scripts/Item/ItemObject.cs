using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{

    public string GetInteractPrompt();
    public void OnInteract();
}


public class ItemObject : MonoBehaviour
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        // CharaterManager ±¸Çö
    }

}
