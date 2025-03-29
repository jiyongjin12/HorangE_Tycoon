using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class DisplayStand : MonoBehaviour
{
    public float interactionDelay = 1f;  // 영역에 머무른 후 전시 시작 시간
    private Coroutine interactionCoroutine;

    public List<Item> items = new List<Item>();
    public int capacity = 15;

    #region 진열대 추가
    public bool AddItem(Item item)
    {
        if (items.Count < capacity)
        {
            items.Add(item);
            Debug.Log(item.itemName + " 진열대에 추가됨. 현재 진열 수: " + items.Count);
            return true;
        }
        Debug.Log("진열대가 가득 찼습니다!");
        return false;
    }
    #endregion

    #region 진열대
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionCoroutine = StartCoroutine(TransferItem(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactionCoroutine != null)
            {
                StopCoroutine(interactionCoroutine);
                interactionCoroutine = null;
            }
        }
    }

    IEnumerator TransferItem(Collider player)
    {
        yield return new WaitForSeconds(interactionDelay);
        while (true)
        {
            PlayerInventorySO holder = player.GetComponent<PlayerInventorySO>();
            if (holder != null && holder != null)
            {
                // 플레이어 인벤토리의 첫 번째 아이템을 전시 (예시)
                if (holder.items.Count > 0)
                {
                    Item item = holder.items[0];
                    if (AddItem(item))
                    {
                        holder.items.RemoveAt(0);
                        Debug.Log(item.itemName + "전.완");
                    }
                    else
                    {
                        Debug.Log("진.꽉");
                    }
                }
                else
                {
                    Debug.Log("아.없");
                }
            }
            yield return new WaitForSeconds(interactionDelay);
        }
    }
    #endregion
}
