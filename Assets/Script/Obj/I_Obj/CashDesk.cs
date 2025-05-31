using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CashDesk : MonoBehaviour
{
    [Header("Checkout Settings")]
    //public Transform processingPoint;
    public float processingDelay = 1f; // 결재 시간

    [Header("Queue Settings")]
    public List<Transform> customerPositions;
    public List<Customer> customerQueue = new List<Customer>();

    private Coroutine processCoroutine;

    // 외부 트리거
    public InteractionPoint Trigger;

    void Start()
    {
        Trigger.OnEntered += HandleEnter;
        Trigger.OnExited += HandleExit;

        for (int i = 0; i < customerPositions.Count; i++)
            customerQueue.Add(null);
    }

    private void OnDisable()
    {
        // 씬 전환이나 비활성화 시 이벤트 언등록
        Trigger.OnEntered -= HandleEnter;
        Trigger.OnExited -= HandleExit;
    }

    void Update()
    {

        if (customerQueue.Count > 0 && customerQueue[0] != null) // 계산 끝
        {
            if (customerQueue[0].inventory.Count == 0)
            {
                customerQueue[0].AllActionCompleted = true; 
                customerQueue[0] = null;
            }
        }

        for (int i = 0; i < customerQueue.Count - 1; i++)
        {
            if (customerQueue[i] == null && customerQueue[i + 1] != null)
            {
                customerQueue[i] = customerQueue[i + 1];
                customerQueue[i + 1] = null;
            }
        }
    }

    public bool EnqueueCustomer(Customer c)
    {
        for (int i = 0; i < customerQueue.Count; i++)
        {
            if (customerQueue[i] == null)
            {
                customerQueue[i] = c;
                return true;
            }
        }
        return false;
    }

    private void HandleEnter(Collider other)
    {
        if (processCoroutine == null)
            processCoroutine = StartCoroutine(ProcessPayments());
    }

    private void HandleExit(Collider other)
    {

        if (processCoroutine != null)
        {
            StopCoroutine(processCoroutine);
            processCoroutine = null;
        }

    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        processCoroutine = StartCoroutine(ProcessPayments());
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        StopCoroutine(processCoroutine);
    //    }
    //}

    private IEnumerator ProcessPayments()
    {
        yield return new WaitForSeconds(processingDelay);
        while (true)
        {
            if (customerQueue[0] != null)
            {
                Customer front = customerQueue[0];
                if (front.inventory.Count > 0)
                {
                    Item item = front.inventory[0];

                    if (MoneyManager.Instance != null)
                        MoneyManager.Instance.AddMoney(item.Cost);

                    front.inventory.RemoveAt(0);
                    Debug.Log(MoneyManager.Instance.currentMoney);
                }
            }
            yield return new WaitForSeconds(processingDelay);
        }
    }
}
