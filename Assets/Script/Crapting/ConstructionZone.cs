using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionZone : MonoBehaviour
{
    public Item requiredMaterial;
    public int requiredQuantity = 5;
    public float interactionDelay = 1f;      // 진입 후 대기 시간
    public float interactionInterval = .5f;   // 재료 사용 간격

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
                    Debug.Log("건설 진행 중: 남은 재료 수 " + requiredQuantity);
                }
                else
                {
                    Debug.Log("필요한 재료가 가방에 없습니다.");
                }
            }
            yield return new WaitForSeconds(interactionInterval);
        }
        constructionComplete = true;
        Debug.Log("건설 완료!");
    }
}
