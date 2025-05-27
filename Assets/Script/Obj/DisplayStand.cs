using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayStand : MonoBehaviour
{
    //public float interactionDelay = 1f;  // 
    //private Coroutine interactionCoroutine;

    //public List<Item> items = new List<Item>();
    //public int capacity = 15;


    //public int CustomerCount = 5;           // 
    //public GameObject CustomerPosPrefab;    // 
    //public Vector3 CustomerPos_Interval = new Vector3(3, 0, 0);

    //public List<Transform> CustomerPosList = new List<Transform>();
    //public List<Customer> customerQueue = new List<Customer>();

    //public bool HaveItem = false;



    //private void Start()
    //{
    //    SetupCustomerPositions();
    //}

    //private void Update()
    //{
    //    UpdateCustomerQueue();
    //    UpdateCustomerPositionVisuals();
    //}


    //#region ������ �߰�
    //public bool AddItem(Item item)
    //{
    //    if (items.Count < capacity)
    //    {
    //        items.Add(item);
    //        Debug.Log(item.itemName + " �����뿡 �߰���. ���� ���� ��: " + items.Count);
    //        return true;
    //    }
    //    return false;
    //}
    //#endregion


    //#region ������
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        interactionCoroutine = StartCoroutine(TransferItem(other));
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        if (interactionCoroutine != null)
    //        {
    //            StopCoroutine(interactionCoroutine);
    //            interactionCoroutine = null;
    //        }
    //    }
    //}

    //IEnumerator TransferItem(Collider player)
    //{
    //    yield return new WaitForSeconds(interactionDelay);
    //    while (true)
    //    {
    //        PlayerInventorySO holder = player.GetComponent<PlayerInventorySO>();
    //        if (holder != null && holder != null)
    //        {
    //            if (holder.items.Count > 0)
    //            {
    //                Item item = holder.items[0];
    //                if (AddItem(item))
    //                {
    //                    //holder.items.RemoveAt(0);
    //                    holder.UesItem(item);
    //                    Debug.Log(item.itemName + "��.��");
    //                }
    //                else
    //                {
    //                    Debug.Log("��.��");
    //                }
    //            }
    //            else
    //            {
    //                Debug.Log("��.��");
    //            }
    //        }
    //        yield return new WaitForSeconds(interactionDelay);
    //    }
    //}
    //#endregion

    //#region Customer Queue & Position Setup
    //void SetupCustomerPositions()
    //{
    //    for (int i = 0; i < CustomerCount; i++)
    //    {
    //        customerQueue.Add(null); // �ʱ�ȭ

    //        Vector3 spawnPos = transform.position + (CustomerPos_Interval * i);
    //        GameObject posObj = Instantiate(CustomerPosPrefab, spawnPos, Quaternion.identity, transform);
    //        posObj.SetActive(false);
    //        CustomerPosList.Add(posObj.transform);
    //    }
    //}

    //public void AddCustomerToQueue(Customer customer)
    //{
    //    for (int i = 0; i < customerQueue.Count; i++)
    //    {
    //        if (customerQueue[i] == null)
    //        {
    //            customerQueue[i] = customer;
    //            UpdateCustomerPositionVisuals();
    //            return;
    //        }
    //    }

    //    Debug.Log("���� �� á���ϴ�.");
    //}

    //private void UpdateCustomerQueue()
    //{
    //    bool moved = false;
    //    for (int i = 0; i < customerQueue.Count - 1; i++)
    //    {
    //        if (customerQueue[i] == null && customerQueue[i + 1] != null)
    //        {
    //            customerQueue[i] = customerQueue[i + 1];
    //            customerQueue[i + 1] = null;
    //            moved = true;
    //        }
    //    }

    //    if (moved)
    //    {
    //        UpdateCustomerPositionVisuals();
    //    }
    //}

    //public void UpdateCustomerPositionVisuals()
    //{
    //    if (!HaveItem)
    //    {
    //        for (int i = 0; i < CustomerPosList.Count; i++)
    //        {
    //            CustomerPosList[i].gameObject.SetActive(false);

    //            customerQueue[i] = null;
    //        }
    //        return;
    //    }
    //    else
    //    {
    //        if (CustomerPosList.Count > 0)
    //        {
    //            CustomerPosList[0].gameObject.SetActive(true);
    //        }
    //    }

    //    for (int i = 0; i < customerQueue.Count; i++)
    //    {
    //        int posIndex = i + 1; 
    //        if (posIndex < CustomerPosList.Count)
    //        {
    //            if (customerQueue[i] != null)
    //            {
    //                CustomerPosList[posIndex].gameObject.SetActive(true);
    //                customerQueue[i].transform.position = CustomerPosList[posIndex].position;
    //            }
    //            else
    //            {
    //                CustomerPosList[posIndex].gameObject.SetActive(false);
    //            }
    //        }
    //    }
    //}


    //#endregion

    [Header("진열대 설정")]
    public List<Item> storedItems = new List<Item>();
    public int capacity = 15;
    public float interactionDelay = 1f;

    private Coroutine transferCoroutine;

    [Header("대기열 설정")]
    public List<Transform> customerPositions;
    [SerializeField]private List<Customer> customerQueue = new List<Customer>();


    private void Start()
    {
        // queue 초기화 (최대 customerPositions.Count 개)
        for (int i = 0; i < customerPositions.Count; i++)
            customerQueue.Add(null);
    }

    private void Update()
    {
        // 줄에 빈 칸이 있으면 자동으로 땡겨서 앞쪽부터 채우기
        bool shifted = false;
        for (int i = 0; i < customerQueue.Count - 1; i++)
        {
            if (customerQueue[i] == null && customerQueue[i + 1] != null)
            {
                customerQueue[i] = customerQueue[i + 1];
                customerQueue[i + 1] = null;
                shifted = true;
            }
        }
        if (shifted) UpdateCustomerPositions();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag("Player"))
        {
            transferCoroutine = StartCoroutine(TransferItem(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (transferCoroutine != null)
            {
                StopCoroutine(transferCoroutine);
                transferCoroutine = null;
            }
        }
    }

    private IEnumerator TransferItem(Collider player)
    {
        yield return new WaitForSeconds(interactionDelay);

        while (true)
        {
            var holder = player.GetComponent<PlayerInventorySO>();
            if (holder != null)
            {
                if (holder.items.Count > 0)
                {
                    Item item = holder.items[0];
                    if (AddItem(item))
                    {
                        holder.UesItem(item);
                        Debug.Log(item.itemName + " 전송됨.");
                    }
                    else
                    {
                        Debug.Log("진열대가 가득 찼습니다.");
                    }
                }
                else
                {
                    Debug.Log("플레이어 인벤토리에 아이템이 없습니다.");
                }
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
                UpdateCustomerPositions();
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
        // Update()에서 자동으로 땡기고 위치 갱신
    }

    // current customerQueue 상태에 맞춰
    // 각 고객을 customerPositions[i] 위치로 이동시키고,
    // 해당 위치 활성화/비활성화 처리
    private void UpdateCustomerPositions()
    {
        for (int i = 0; i < customerPositions.Count; i++)
        {
            var pos = customerPositions[i];
            var cust = (i < customerQueue.Count) ? customerQueue[i] : null;

            // 고객이 있으면 위치 활성화, 없으면 비활성화
            pos.gameObject.SetActive(cust != null);

            if (cust != null)
            {
                // NavMeshAgent 이동 또는 단순 포지션 세팅
                //cust.MoveTo(pos.position);
            }
        }
    }

    #endregion
}
