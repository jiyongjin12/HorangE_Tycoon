using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventorySO : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public int maxCapacity = 10;

    public GameObject Player_Hand;
    public float itemStackOffset = 0.3f;

    [SerializeField]
    private List<GameObject> spawnedItems = new List<GameObject>();

    public bool AddItem(Item item)
    {
        if (items.Count < maxCapacity)
        {
            items.Add(item);
            if (item.Item_Prefab != null && Player_Hand != null)
            {
                GameObject spawnedItem = Instantiate(item.Item_Prefab, Player_Hand.transform);

                spawnedItem.transform.localPosition = new Vector3(0, items.Count * itemStackOffset, 0);
                spawnedItems.Add(spawnedItem);
            }
            return true;
        }

        return false;
    }

    public bool UesItem(Item item)
    {
        if (items.Contains(item))
        {
            int index = items.IndexOf(item);
            items.RemoveAt(index);
            
            if (spawnedItems.Count > index)
            {
                Destroy(spawnedItems[index]);
                spawnedItems.RemoveAt(index);
            }

            for (int i = 0; i < spawnedItems.Count; i++)
            {
                spawnedItems[i].transform.localPosition = new Vector3(0, (i + 1) * itemStackOffset, 0);
            }
            return true;
        }
        return false;
    }
}
