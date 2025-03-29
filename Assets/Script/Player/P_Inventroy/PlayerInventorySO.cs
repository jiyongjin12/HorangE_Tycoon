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

    // 인벤토리에 추가된 아이템의 스폰된 프리팹을 관리하는 리스트
    [SerializeField]
    private List<GameObject> spawnedItems = new List<GameObject>();

    public bool AddItem(Item item)
    {
        if (items.Count < maxCapacity)
        {
            items.Add(item);
            Debug.Log("아.추");
            if (item.Item_Prefab != null && Player_Hand != null)
            {
                // Player_Hand를 부모로 하여 프리팹 인스턴스 생성
                GameObject spawnedItem = Instantiate(item.Item_Prefab, Player_Hand.transform);
                // 스폰된 아이템의 로컬 위치를 인벤토리 순서에 따라 설정
                // 인덱스 0: Player_Hand의 기준 위치, 그 이후로 Y축 오프셋을 더함
                spawnedItem.transform.localPosition = new Vector3(0, items.Count * itemStackOffset, 0);
                spawnedItems.Add(spawnedItem);
            }
            return true;
        }
        Debug.Log("인.꽉");
        return false;
    }

    public bool UesItem(Item item)
    {
        #region 예전 세이브용
        //if (items.Contains(item))
        //{
        //    int index = items.IndexOf(item);
        //    items.Remove(item);
        //    Debug.Log("아.사");
        //    // 인벤토리에서 제거된 아이템에 대응하는 스폰된 오브젝트도 제거
        //    if (spawnedItems.Count > index)
        //    {
        //        Destroy(spawnedItems[index]);
        //        spawnedItems.RemoveAt(index);
        //    }
        //    // 남은 아이템들의 스택 위치를 다시 재조정 (인벤토리 순서대로 쌓임)
        //    for (int i = 0; i < spawnedItems.Count; i++)
        //    {
        //        spawnedItems[i].transform.localPosition = new Vector3(0, (i + 1) * itemStackOffset, 0);
        //    }
        //    return true;
        //}
        //Debug.Log("가.없");
        //return false;
        #endregion

        if (items.Contains(item))
        {
            int index = items.IndexOf(item);
            items.RemoveAt(index);
            Debug.Log("아.사");
            // 인벤토리에서 제거된 아이템에 대응하는 스폰된 오브젝트도 제거
            if (spawnedItems.Count > index)
            {
                Destroy(spawnedItems[index]);
                spawnedItems.RemoveAt(index);
            }
            // 남은 아이템들의 스택 위치를 다시 재조정 (인벤토리 순서대로 쌓임)
            for (int i = 0; i < spawnedItems.Count; i++)
            {
                spawnedItems[i].transform.localPosition = new Vector3(0, (i + 1) * itemStackOffset, 0);
            }
            return true;
        }
        Debug.Log("가.없");
        return false;
    }
}
