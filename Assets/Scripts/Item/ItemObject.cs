using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{

    public string GetInteractPrompt();
    public void OnInteract();
}


public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData itemdata;

    public string GetInteractPrompt()
    {
        string str = $"{itemdata.displayName}\n{itemdata.description}";
        return str;
    }

    public void OnInteract()
    {
        Destroy(gameObject);
    }

}
