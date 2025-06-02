using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DisplayStand : MonoBehaviour
{
    [Header("진열대 설정")]
    public List<Item> storedItems = new List<Item>();
    public int capacity = 15;
    public float interactionDelay = 1f;

    private Coroutine transferCoroutine;

    [Header("대기열 설정")]
    public List<Transform> customerPositions;
    public List<Customer> customerQueue = new List<Customer>();

    // 외부 트리거
    public InteractionPoint Trigger;

    private void Start()
    {
        Trigger.OnEntered += HandleEnter;
        Trigger.OnExited += HandleExit;

        // customerQueue 초기화
        for (int i = 0; i < customerPositions.Count; i++)
            customerQueue.Add(null);
    }

    private void OnDisable()
    {
        // 씬 전환이나 비활성화 시 이벤트 언등록
        Trigger.OnEntered -= HandleEnter;
        Trigger.OnExited -= HandleExit;
    }

    private void Update()
    {

        // 쇼핑 완료한 손님 제거
        for (int i = 0; i < customerQueue.Count; i++)
        {
            var c = customerQueue[i];
            if (c != null && c.shoppingCompleted)
            {
                customerQueue[i] = null;
            }
        }

        // 줄에 빈 칸이 있을때 앞쪽부터 채우기
        for (int i = 0; i < customerQueue.Count - 1; i++)
        {
            if (customerQueue[i] == null && customerQueue[i + 1] != null)
            {
                customerQueue[i] = customerQueue[i + 1];
                customerQueue[i + 1] = null;
            }
        }
    }

    private void HandleEnter(Collider other)
    {
        if (transferCoroutine == null)
            transferCoroutine = StartCoroutine(TransferItem(other));
    }

    private void HandleExit(Collider other)
    {

        if (transferCoroutine != null)
        {
            StopCoroutine(transferCoroutine);
            transferCoroutine = null;
        }

    }

    private IEnumerator TransferItem(Collider player)
    {
        yield return new WaitForSeconds(interactionDelay);

        while (true)
        {
            var holder = player.GetComponent<PlayerInventorySO>();
            if (holder != null && holder.items.Count > 0 && storedItems.Count < capacity)
            {
                // 아이템 옮기기
                Item item = holder.items[0];
                storedItems.Add(item);
                holder.UesItem(item);
                Debug.Log($"{item.itemName} 전송됨.");
            }
            yield return new WaitForSeconds(interactionDelay);
        }
    }

    public bool AddItem(Item item)
    {
        if (storedItems.Count < capacity)
        {
            storedItems.Add(item);
            Debug.Log(item.itemName + " 진열대에 저장됨. 현재 개수: " + storedItems.Count);
            return true;
        }
        return false;
    }

    #region 아이템 ID 확인
    // 맨 앞(0번) 아이템의 id를 반환, 없으면 -1
    public int GetFrontItemId()
    {
        return storedItems.Count > 0 ? storedItems[0].id : -1;
    }

    // 맨 앞(0번) 아이템 제거
    public void ServeFrontItem()
    {
        if (storedItems.Count > 0)
            storedItems.RemoveAt(0);
    }
    #endregion


    #region 고객 대기열 관리
    // 빈 슬롯이 있으면 고객을 줄 뒤쪽에 추가. 성공 시 true 반환.
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
        return false; // 줄이 가득 참
    }


    // 줄 맨 앞(0번) 고객 제거 후 줄 자동 정렬
    public void DequeueFrontCustomer()
    {
        if (customerQueue[0] == null) return;
        customerQueue[0] = null;
    }

    #endregion
}
