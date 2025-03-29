using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialZone : MonoBehaviour
{
    public Item materialToCollect;
    public float interactionDelay = 1f;      // ���� �� ��� �ð�
    public float interactionInterval = .5f;   // ��� �߰� ����

    private Coroutine interactionCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionCoroutine = StartCoroutine(StartInteractionAfterDelay(other));
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

    IEnumerator StartInteractionAfterDelay(Collider player)
    {
        yield return new WaitForSeconds(interactionDelay);
        while (true)
        {
            PlayerInventorySO holder = player.GetComponent<PlayerInventorySO>();
            if (holder != null && holder != null)
            {
                holder.AddItem(materialToCollect);
            }
            yield return new WaitForSeconds(interactionInterval);
        }
    }
}
