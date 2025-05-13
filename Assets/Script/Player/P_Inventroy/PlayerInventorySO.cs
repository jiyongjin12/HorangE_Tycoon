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

    // �κ��丮�� �߰��� �������� ������ �������� �����ϴ� ����Ʈ
    [SerializeField]
    private List<GameObject> spawnedItems = new List<GameObject>();

    public bool AddItem(Item item)
    {
        if (items.Count < maxCapacity)
        {
            items.Add(item);
            Debug.Log("��.��");
            if (item.Item_Prefab != null && Player_Hand != null)
            {
                // Player_Hand�� �θ�� �Ͽ� ������ �ν��Ͻ� ����
                GameObject spawnedItem = Instantiate(item.Item_Prefab, Player_Hand.transform);
                // ������ �������� ���� ��ġ�� �κ��丮 ������ ���� ����
                // �ε��� 0: Player_Hand�� ���� ��ġ, �� ���ķ� Y�� �������� ����
                spawnedItem.transform.localPosition = new Vector3(0, items.Count * itemStackOffset, 0);
                spawnedItems.Add(spawnedItem);
            }
            return true;
        }
        Debug.Log("��.��");
        return false;
    }

    public bool UesItem(Item item)
    {
        #region ���� ���̺��
        //if (items.Contains(item))
        //{
        //    int index = items.IndexOf(item);
        //    items.Remove(item);
        //    Debug.Log("��.��");
        //    // �κ��丮���� ���ŵ� �����ۿ� �����ϴ� ������ ������Ʈ�� ����
        //    if (spawnedItems.Count > index)
        //    {
        //        Destroy(spawnedItems[index]);
        //        spawnedItems.RemoveAt(index);
        //    }
        //    // ���� �����۵��� ���� ��ġ�� �ٽ� ������ (�κ��丮 ������� ����)
        //    for (int i = 0; i < spawnedItems.Count; i++)
        //    {
        //        spawnedItems[i].transform.localPosition = new Vector3(0, (i + 1) * itemStackOffset, 0);
        //    }
        //    return true;
        //}
        //Debug.Log("��.��");
        //return false;
        #endregion

        if (items.Contains(item))
        {
            int index = items.IndexOf(item);
            items.RemoveAt(index);
            Debug.Log("��.��");
            // �κ��丮���� ���ŵ� �����ۿ� �����ϴ� ������ ������Ʈ�� ����
            if (spawnedItems.Count > index)
            {
                Destroy(spawnedItems[index]);
                spawnedItems.RemoveAt(index);
            }
            // ���� �����۵��� ���� ��ġ�� �ٽ� ������ (�κ��丮 ������� ����)
            for (int i = 0; i < spawnedItems.Count; i++)
            {
                spawnedItems[i].transform.localPosition = new Vector3(0, (i + 1) * itemStackOffset, 0);
            }
            return true;
        }
        Debug.Log("��.��");
        return false;
    }
}
