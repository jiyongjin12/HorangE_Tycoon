using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class RiceCakeMachion : MonoBehaviour
{
    public Item tteokItem;
    public float interactionDelay = 1f;      // 아이템 집기 시간 
    private Coroutine productionCoroutine;

    // 외부 트리거
    public InteractionPoint Trigger;
    public bool Check = false;

    private void Start()
    {
        Trigger.OnEntered += HandleEnter;
        Trigger.OnExited += HandleExit;

        Debug.Log("이벤트 연결 완료!");
    }

    private void OnDisable()
    {
        // 씬 전환이나 비활성화 시 이벤트 언등록
        Trigger.OnEntered -= HandleEnter;
        Trigger.OnExited -= HandleExit;
    }

    private void HandleEnter(Collider other)
    {
        Check = true;
        if (productionCoroutine == null)
            productionCoroutine = StartCoroutine(ProduceRiceCake(other));
    }

    private void HandleExit(Collider other)
    {

        if (productionCoroutine != null)
        {
            Check = false;
            StopCoroutine(productionCoroutine);
            productionCoroutine = null;
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
                holder.AddItem(tteokItem);
            }
            yield return new WaitForSeconds(interactionDelay);
        }
    }
}
