using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class RiceCakeMachion : MonoBehaviour
{
    public Item tteokItem;              // 생산할 떡 아이템
    public float interactionDelay = 1f;     // 영역 진입 후 대기 시간
    public float productionInterval = 1.5f; // 생산 간격
    private Coroutine productionCoroutine;

    private bool machineConstructed = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && machineConstructed)
        {
            productionCoroutine = StartCoroutine(ProduceRiceCake(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (productionCoroutine != null)
            {
                StopCoroutine(productionCoroutine);
                productionCoroutine = null;
            }
        }
    }

    IEnumerator ProduceRiceCake(Collider player)
    {
        yield return new WaitForSeconds(interactionDelay);
        while (true)
        {
            PlayerInventorySO holder = player.GetComponent<PlayerInventorySO>();
            if (holder != null && holder != null)
            {
                if (holder.AddItem(tteokItem))
                {
                    Debug.Log("떡.추");
                }
                else
                {
                    Debug.Log("인.꽉");
                }
            }
            yield return new WaitForSeconds(productionInterval);
        }
    }
}
