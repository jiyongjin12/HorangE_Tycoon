using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class DisplayStand : MonoBehaviour
{
    public float interactionDelay = 1f;  // ������ �ӹ��� �� ���� ���� �ð�
    private Coroutine interactionCoroutine;

    public List<Item> items = new List<Item>();
    public int capacity = 15;

    #region ������ �߰�
    public bool AddItem(Item item)
    {
        if (items.Count < capacity)
        {
            items.Add(item);
            Debug.Log(item.itemName + " �����뿡 �߰���. ���� ���� ��: " + items.Count);
            return true;
        }
        Debug.Log("�����밡 ���� á���ϴ�!");
        return false;
    }
    #endregion

    #region ������
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
                // �÷��̾� �κ��丮�� ù ��° �������� ���� (����)
                if (holder.items.Count > 0)
                {
                    Item item = holder.items[0];
                    if (AddItem(item))
                    {
                        holder.items.RemoveAt(0);
                        Debug.Log(item.itemName + "��.��");
                    }
                    else
                    {
                        Debug.Log("��.��");
                    }
                }
                else
                {
                    Debug.Log("��.��");
                }
            }
            yield return new WaitForSeconds(interactionDelay);
        }
    }
    #endregion
}
