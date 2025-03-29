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
            Debug.Log(item.itemName + " �߰���. ���� ������ ��: " + items.Count);
            return true;
        }
        Debug.Log("������ ���� á���ϴ�!");
        return false;
    }

    public bool RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log(item.itemName + " ���ŵ�. ���� ������ ��: " + items.Count);
            return true;
        }
        Debug.Log("�ش� �������� ���濡 �����ϴ�.");
        return false;
    }
}
