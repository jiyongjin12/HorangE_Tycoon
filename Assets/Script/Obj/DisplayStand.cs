using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class DisplayStand : MonoBehaviour
{
    public float interactionDelay = 1f;  // 머무른 후 전시 시작
    private Coroutine interactionCoroutine;

    public List<Item> items = new List<Item>();
    public int capacity = 15;


    public int CustomerCount = 5;           // 줄의 최대 크기
    public GameObject CustomerPosPrefab;    // 대기 위치 프리팹
    public Vector3 CustomerPos_Interval = new Vector3(3, 0, 0);

    public List<Transform> CustomerPosList = new List<Transform>();
    public List<Customer> customerQueue = new List<Customer>();

    public bool HaveItem = false;



    private void Start()
    {
        SetupCustomerPositions();
    }

    private void Update()
    {
        // 빈 슬롯이 있으면 앞으로 당김
        UpdateCustomerQueue();
        UpdateCustomerPositionVisuals();
    }


    #region 진열대 추가
    public bool AddItem(Item item)
    {
        if (items.Count < capacity)
        {
            items.Add(item);
            Debug.Log(item.itemName + " 진열대에 추가됨. 현재 진열 수: " + items.Count);
            return true;
        }
        return false;
    }
    #endregion


    #region 진열대
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionCoroutine = StartCoroutine(TransferItem(other));
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

    IEnumerator TransferItem(Collider player)
    {
        yield return new WaitForSeconds(interactionDelay);
        while (true)
        {
            PlayerInventorySO holder = player.GetComponent<PlayerInventorySO>();
            if (holder != null && holder != null)
            {
                // 플레이어 인벤토리의 첫 번째 아이템을 전시
                if (holder.items.Count > 0)
                {
                    Item item = holder.items[0];
                    if (AddItem(item))
                    {
                        //holder.items.RemoveAt(0);
                        holder.UesItem(item);
                        Debug.Log(item.itemName + "전.완");
                    }
                    else
                    {
                        Debug.Log("진.꽉");
                    }
                }
                else
                {
                    Debug.Log("아.없");
                }
            }
            yield return new WaitForSeconds(interactionDelay);
        }
    }
    #endregion

    #region Customer Queue & Position Setup
    void SetupCustomerPositions()
    {
        // 위치 프리팹 생성
        for (int i = 0; i < CustomerCount; i++)
        {
            customerQueue.Add(null); // 초기화

            Vector3 spawnPos = transform.position + (CustomerPos_Interval * i);
            GameObject posObj = Instantiate(CustomerPosPrefab, spawnPos, Quaternion.identity, transform);
            posObj.SetActive(false);
            CustomerPosList.Add(posObj.transform);
        }
    }

    public void AddCustomerToQueue(Customer customer)
    {
        // 첫 빈 슬롯을 찾아 손님 추가
        for (int i = 0; i < customerQueue.Count; i++)
        {
            if (customerQueue[i] == null)
            {
                customerQueue[i] = customer;
                UpdateCustomerPositionVisuals();
                return;
            }
        }

        Debug.Log("줄이 꽉 찼습니다.");
    }

    private void UpdateCustomerQueue()
    {
        bool moved = false;
        // 빈 슬롯이 있으면 뒤의 손님들을 앞으로 당겨옴
        for (int i = 0; i < customerQueue.Count - 1; i++)
        {
            if (customerQueue[i] == null && customerQueue[i + 1] != null)
            {
                customerQueue[i] = customerQueue[i + 1];
                customerQueue[i + 1] = null;
                moved = true;
            }
        }

        if (moved)
        {
            UpdateCustomerPositionVisuals();
        }
    }

    public void UpdateCustomerPositionVisuals()
    {
        if (!HaveItem)
        {
            // HaveItem false이면, 전체 CustomerPosList 비활성화
            for (int i = 0; i < CustomerPosList.Count; i++)
            {
                CustomerPosList[i].gameObject.SetActive(false);

                customerQueue[i] = null;
            }
            return;
        }
        else
        {
            // HaveItem이 true라면 CustomerPosList[0]은 항상 활성화
            if (CustomerPosList.Count > 0)
            {
                CustomerPosList[0].gameObject.SetActive(true);
            }
        }

        // customerQueue에 있는 손님은 CustomerPosList[1]부터 배정
        for (int i = 0; i < customerQueue.Count; i++)
        {
            int posIndex = i + 1; // customerQueue[0] → CustomerPosList[1], ...
            if (posIndex < CustomerPosList.Count)
            {
                if (customerQueue[i] != null)
                {
                    CustomerPosList[posIndex].gameObject.SetActive(true);
                    // 손님 오브젝트의 위치를 배정된 CustomerPosList로 업데이트
                    customerQueue[i].transform.position = CustomerPosList[posIndex].position;
                }
                else
                {
                    CustomerPosList[posIndex].gameObject.SetActive(false);
                }
            }
        }
    }

    
    #endregion

}
