using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class RiceCakeMachion : MonoBehaviour
{
    public Item tteokItem;              // ������ �� ������
    public float interactionDelay = 1f;     // ���� ���� �� ��� �ð�
    public float productionInterval = 1.5f; // ���� ����
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
                    Debug.Log("��.��");
                }
                else
                {
                    Debug.Log("��.��");
                }
            }
            yield return new WaitForSeconds(productionInterval);
        }
    }
}
