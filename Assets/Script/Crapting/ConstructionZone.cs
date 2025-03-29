using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionZone : MonoBehaviour
{
    public Item requiredMaterial;
    public int requiredQuantity = 5;
    public float interactionDelay = 1f;      // ���� �� ��� �ð�
    public float interactionInterval = .5f;   // ��� ��� ����

    private Coroutine interactionCoroutine;
    private bool constructionComplete = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !constructionComplete)
        {
            interactionCoroutine = StartCoroutine(StartConstructionAfterDelay(other));
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

    IEnumerator StartConstructionAfterDelay(Collider player)
    {
        yield return new WaitForSeconds(interactionDelay);
        while (requiredQuantity > 0)
        {
            PlayerInventorySO holder = player.GetComponent<PlayerInventorySO>();
            if (holder != null && holder != null)
            {
                if (holder.items.Contains(requiredMaterial))
                {
                    holder.RemoveItem(requiredMaterial);
                    requiredQuantity--;
                    Debug.Log("�Ǽ� ���� ��: ���� ��� �� " + requiredQuantity);
                }
                else
                {
                    Debug.Log("�ʿ��� ��ᰡ ���濡 �����ϴ�.");
                }
            }
            yield return new WaitForSeconds(interactionInterval);
        }
        constructionComplete = true;
        Debug.Log("�Ǽ� �Ϸ�!");
    }
}
