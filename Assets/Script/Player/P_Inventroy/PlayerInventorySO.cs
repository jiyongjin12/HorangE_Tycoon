using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventorySO : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public int maxCapacity = 10;

    public bool AddItem(Item item)
    {
        if (items.Count < maxCapacity)
        {
            items.Add(item);
            Debug.Log("��.��");
            return true;
        }
        Debug.Log("��.��");
        return false;
    }

    public bool UesItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log("��.��");
            return true;
        }
        Debug.Log("��.��");
        return false;
    }
}
