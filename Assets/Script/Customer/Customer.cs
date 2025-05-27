using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Customer : MonoBehaviour
{
    [Header("Order Settings")]
    public int neededItemId;
    public int neededCount;

    [Header("Collected Items")]
    public List<Item> inventory = new List<Item>();

    [Header("State Flags")]
    public bool shoppingCompleted = false;

    private NavMeshAgent agent;
    private DisplayStand targetStand;
    private Coroutine collectCoroutine;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!shoppingCompleted)
        {
            MoveInQueue();
        }
        else
        {
            // agent.SetDestination(nextDestination); 계산대로 이동 예정
        }
    }

    private void FixedUpdate()
    {
        SeekStand();
    }

    private void MoveInQueue()
    {
        if (targetStand == null) return;

        int myIndex = targetStand.customerQueue.IndexOf(this);
        if (myIndex < 0) return;

        Transform slot = targetStand.customerPositions[myIndex];
        if (agent.destination != slot.position)
        {
            agent.SetDestination(slot.position);
        }

        // 맨 앞 자리에 도착하면 수집 시작
        if (myIndex == 0 && collectCoroutine == null && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            collectCoroutine = StartCoroutine(CollectItems());
        }
    }

    void SeekStand()
    {
        // 씬의 모든 Stand 중에서 원하는 item을 파는 곳 찾기
        foreach (var stand in FindObjectsOfType<DisplayStand>())
        {
            if (stand.storedItems.Exists(i => i.id == neededItemId))
            {
                if (stand.EnqueueCustomer(this))
                {
                    targetStand = stand;
                    return;
                }
            }
        }
    }

    void OnCollisionStay(Collision col) // 충돌관련 처리 ( 충돌 후 강제 속도 초기화 )
    {
        if (agent != null)
        {
            agent.velocity = agent.desiredVelocity;
        }

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }


    IEnumerator CollectItems()
    {
        while (inventory.Count < neededCount)
        {
            int frontId = targetStand.GetFrontItemId();
            if (frontId == neededItemId)
            {
                Item item = targetStand.storedItems[0];
                targetStand.ServeFrontItem();
                inventory.Add(item);
                Debug.Log("아이템 수집: ID " + neededItemId + ", 총 " + inventory.Count);
            }
            else
            {
                // 원하는 아이템이 아니면 줄에서 빠져 나와 뒤로
                targetStand.DequeueFrontCustomer();
                targetStand.EnqueueCustomer(this);
                yield break;
            }
            yield return new WaitForSeconds(1f);
        }
        // 모두 수집 완료
        shoppingCompleted = true;
        agent.ResetPath(); // 이게 경로 제거인듯?
        targetStand.DequeueFrontCustomer();
        Debug.Log("쇼핑 완료: ID " + neededItemId);
    }
}
