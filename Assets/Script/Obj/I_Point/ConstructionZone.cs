using System.Collections;
using UnityEngine;

public class ConstructionZone : MonoBehaviour
{
    public int machineCost = 200;         
    public float interactionDelay = 1f;
    private Coroutine interactionCoroutine;

    public GameObject RiceCakeMachion;

    private void Start()
    {
        if (RiceCakeMachion != null) RiceCakeMachion.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
        if (MoneyManager.Instance != null)
        {
            if (MoneyManager.Instance.SpendMoney(machineCost))
            {
                RiceCakeMachion.gameObject.SetActive(true);

                gameObject.SetActive(false);
            }
        }
    }
}
