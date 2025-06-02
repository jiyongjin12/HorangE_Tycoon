using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Customer : MonoBehaviour
{
    public float PickUpItemTime;  // 진열대 처리는 손님쪽에서, 계산대는 계산대쪽에서 각자 기능 수행 ( 왜 이렇게 했을까... )

    [Header("Order Settings")]
    public int neededItemId;
    public int neededCount;
    public Item needItem;

    [Header("Collected Items")]
    public List<Item> inventory = new List<Item>();

    [Header("State Flags")]
    public bool shoppingCompleted = false;
    public bool AllActionCompleted = false;

    public Transform Exit;

    private NavMeshAgent agent;
    private DisplayStand targetStand1;
    private CashDesk targetStand2;

    private Coroutine collectCoroutine;
    private Coroutine hideCanvasDCoroutine;

    [Header("UI_Display")]
    public bool ShowCanvas_D = false;
    public GameObject Base_D;
    public Image ItemSprite_D;
    public TMP_Text CountText_D;
    public GameObject CheckImage_D;

    [Header("UI_CostTable")]
    public bool ShowCanvas_C = false;
    public GameObject Base_C;
    public Image ItemSprite_C;
    public TMP_Text CountText_C;
    public GameObject CheckImage_C;

    private bool tes = false;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        CheckImage_D.transform.localScale = Vector3.zero;
        CheckImage_C.transform.localScale = Vector3.zero;
        //Base_D.SetActive(false);
        //Base_C.SetActive(false);
    }

    void Update()
    {
        if (AllActionCompleted)
        {
            if (!agent.hasPath || agent.destination != Exit.position)
                agent.SetDestination(Exit.position);

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (!shoppingCompleted)
            {
                if (targetStand1 == null)
                    FindDisplay();
                MoveInDisplay();
            }
            else
            {
                if (targetStand2 == null)
                    FindeCashDesk();
                MoveInCashDesk();
            }
        }

        if (ShowCanvas_D)
        {
            Base_D.SetActive(true);
            UISetting_D();
        }
        if (ShowCanvas_C)
        {
            Base_C.SetActive(true);
            UISetting_C();
        }

    }

    void UISetting_D()
    {
        int a = neededCount - inventory.Count;

        ItemSprite_D.sprite = needItem.TuckImage;
        CountText_D.text = a.ToString();

        if (a == 0)
            ShowCanvas_D = false;


        if(!ShowCanvas_D && !tes)
        {
            hideCanvasDCoroutine = StartCoroutine(AnimateCheckAndHide(CheckImage_D));
            tes = true;
        }
    }

    void UISetting_C()
    {
        ItemSprite_C.sprite = needItem.TuckImage;

        CountText_C.text = inventory.Count.ToString();

        if (inventory.Count == 0)
            ShowCanvas_C = false;

        if (!ShowCanvas_C && !tes)
        {
            hideCanvasDCoroutine = StartCoroutine(AnimateCheckAndHide(CheckImage_C));
            tes = true;
        }
    }


    private IEnumerator AnimateCheckAndHide(GameObject Obj)
    {
        Obj.SetActive(true);

        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            Vector3 scale = Vector3.one * smoothT;
            Obj.transform.localScale = scale;
            tes = false;
            yield return null;
        }


        Base_D.SetActive(false);

        hideCanvasDCoroutine = null;
    }


    private void MoveInDisplay()
    {
        if (targetStand1 == null) return;

        int myIndex = targetStand1.customerQueue.IndexOf(this);
        if (myIndex < 0) return;
            
        Transform slot = targetStand1.customerPositions[myIndex];
        if (agent.destination != slot.position) 
        {
            agent.SetDestination(slot.position);
        }

        // 맨 앞 자리에 도착하면 수집 시작
        if (myIndex == 0 && collectCoroutine == null && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Debug.Log("Check");
            collectCoroutine = StartCoroutine(CollectItems());
        }

        if (myIndex == 0)
            ShowCanvas_D = true;

    }

    void FindDisplay()
    {
        foreach (var stand in FindObjectsOfType<DisplayStand>())
        {
            if (stand.EnqueueCustomer(this))
            {
                targetStand1 = stand;
                return;
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

    IEnumerator CollectItems() // 진열대 처리
    {
        while (inventory.Count < neededCount)
        {
            // 만약 진열대에 재고가 없다면 잠시 대기 후 재시도
            if (targetStand1.storedItems.Count == 0)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }
            int frontId = targetStand1.GetFrontItemId();
            if (frontId == neededItemId)
            {
                var item = targetStand1.storedItems[0];
                targetStand1.ServeFrontItem();
                inventory.Add(item);
                Debug.Log("아이템 수집: ID " + neededItemId + ", 총 " + inventory.Count);
            }
            else
            {
                // 다른 아이템이 앞에 있으면 줄 뒤로 이동
                targetStand1.DequeueFrontCustomer();
                targetStand1.EnqueueCustomer(this);
                collectCoroutine = null;
                yield break;
            }
            yield return new WaitForSeconds(PickUpItemTime);
        }
        shoppingCompleted = true;
        agent.ResetPath();
        targetStand1.DequeueFrontCustomer();
        collectCoroutine = null;
        Debug.Log("쇼핑 완료: ID " + neededItemId);
    }


    private void MoveInCashDesk()
    {
        if (targetStand2 == null) return;

        int idx = targetStand2.customerQueue.IndexOf(this);
        if (idx < 0) return;
        var slot = targetStand2.customerPositions[idx];
        if (!agent.hasPath || agent.destination != slot.position)
            agent.SetDestination(slot.position);

        if (idx == 0)
            ShowCanvas_C = true;
    }

    private void FindeCashDesk()
    {
        foreach (var desk in FindObjectsOfType<CashDesk>())
        {
            targetStand2 = desk;
            targetStand2.EnqueueCustomer(this);
            Debug.Log("확");
            return;
        }
    }
}
