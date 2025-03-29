using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class ConstructionZone : MonoBehaviour
{
    public int machineCost = 200;         // 제작 비용
    public float interactionDelay = 1f;     // 진입 후 대기 시간
    public float interactionInterval = 0.1f;  // 돈 소비 간격
    private Coroutine interactionCoroutine;
    private bool constructionComplete = false;

    public GameObject RiceCakeMachion;

    private void Start()
    {
        if (RiceCakeMachion != null) RiceCakeMachion.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !constructionComplete)
        {
            interactionCoroutine = StartCoroutine(StartConstruction(other));
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

    IEnumerator StartConstruction(Collider player)
    {
        yield return new WaitForSeconds(interactionDelay);
        while (!constructionComplete)
        {
            if (MoneyManager.Instance != null)
            {
                if (MoneyManager.Instance.SpendMoney(machineCost))
                {
                    constructionComplete = true;
                    RiceCakeMachion.gameObject.SetActive(true);
                    Debug.Log("기.제.끝");

                    gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("돈.없");
                }
            }
            yield return new WaitForSeconds(interactionInterval);
        }
    }
}
