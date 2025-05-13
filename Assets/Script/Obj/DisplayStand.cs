using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class DisplayStand : MonoBehaviour
{
    public float interactionDelay = 1f;  // �ӹ��� �� ���� ����
    private Coroutine interactionCoroutine;

    public List<Item> items = new List<Item>();
    public int capacity = 15;


    public int CustomerCount = 5;           // ���� �ִ� ũ��
    public GameObject CustomerPosPrefab;    // ��� ��ġ ������
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
        // �� ������ ������ ������ ���
        UpdateCustomerQueue();
        UpdateCustomerPositionVisuals();
    }


    #region ������ �߰�
    public bool AddItem(Item item)
    {
        if (items.Count < capacity)
        {
            items.Add(item);
            Debug.Log(item.itemName + " �����뿡 �߰���. ���� ���� ��: " + items.Count);
            return true;
        }
        return false;
    }
    #endregion


    #region ������
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
                // �÷��̾� �κ��丮�� ù ��° �������� ����
                if (holder.items.Count > 0)
                {
                    Item item = holder.items[0];
                    if (AddItem(item))
                    {
                        //holder.items.RemoveAt(0);
                        holder.UesItem(item);
                        Debug.Log(item.itemName + "��.��");
                    }
                    else
                    {
                        Debug.Log("��.��");
                    }
                }
                else
                {
                    Debug.Log("��.��");
                }
            }
            yield return new WaitForSeconds(interactionDelay);
        }
    }
    #endregion

    #region Customer Queue & Position Setup
    void SetupCustomerPositions()
    {
        // ��ġ ������ ����
        for (int i = 0; i < CustomerCount; i++)
        {
            customerQueue.Add(null); // �ʱ�ȭ

            Vector3 spawnPos = transform.position + (CustomerPos_Interval * i);
            GameObject posObj = Instantiate(CustomerPosPrefab, spawnPos, Quaternion.identity, transform);
            posObj.SetActive(false);
            CustomerPosList.Add(posObj.transform);
        }
    }

    public void AddCustomerToQueue(Customer customer)
    {
        // ù �� ������ ã�� �մ� �߰�
        for (int i = 0; i < customerQueue.Count; i++)
        {
            if (customerQueue[i] == null)
            {
                customerQueue[i] = customer;
                UpdateCustomerPositionVisuals();
                return;
            }
        }

        Debug.Log("���� �� á���ϴ�.");
    }

    private void UpdateCustomerQueue()
    {
        bool moved = false;
        // �� ������ ������ ���� �մԵ��� ������ ��ܿ�
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
            // HaveItem false�̸�, ��ü CustomerPosList ��Ȱ��ȭ
            for (int i = 0; i < CustomerPosList.Count; i++)
            {
                CustomerPosList[i].gameObject.SetActive(false);

                customerQueue[i] = null;
            }
            return;
        }
        else
        {
            // HaveItem�� true��� CustomerPosList[0]�� �׻� Ȱ��ȭ
            if (CustomerPosList.Count > 0)
            {
                CustomerPosList[0].gameObject.SetActive(true);
            }
        }

        // customerQueue�� �ִ� �մ��� CustomerPosList[1]���� ����
        for (int i = 0; i < customerQueue.Count; i++)
        {
            int posIndex = i + 1; // customerQueue[0] �� CustomerPosList[1], ...
            if (posIndex < CustomerPosList.Count)
            {
                if (customerQueue[i] != null)
                {
                    CustomerPosList[posIndex].gameObject.SetActive(true);
                    // �մ� ������Ʈ�� ��ġ�� ������ CustomerPosList�� ������Ʈ
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
