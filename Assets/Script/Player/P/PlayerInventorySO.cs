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
            Debug.Log(item.itemName + " 추가됨. 현재 아이템 수: " + items.Count);
            return true;
        }
        Debug.Log("가방이 가득 찼습니다!");
        return false;
    }

    public bool RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log(item.itemName + " 제거됨. 남은 아이템 수: " + items.Count);
            return true;
        }
        Debug.Log("해당 아이템이 가방에 없습니다.");
        return false;
    }
}
