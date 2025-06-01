using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class RiceCakeMachion : MonoBehaviour
{
    //public Item tteokItem;
    //public float interactionDelay = 1f;      // 아이템 
    //private Coroutine productionCoroutine;

    //// 제작 트리거
    //public InteractionPoint Trigger;

    //// 추가할 부분


    //private void Start()
    //{
    //    Trigger.OnEntered += HandleEnter;
    //    Trigger.OnExited += HandleExit;

    //    Debug.Log("이벤트 연결 완료!");
    //}

    //private void OnDisable()
    //{
    //    // 씬 전환이나 비활성화 시 이벤트 언등록
    //    Trigger.OnEntered -= HandleEnter;
    //    Trigger.OnExited -= HandleExit;
    //}

    //private void HandleEnter(Collider other)
    //{
    //    if (productionCoroutine == null)
    //        productionCoroutine = StartCoroutine(ProduceRiceCake(other));
    //}

    //private void HandleExit(Collider other)
    //{

    //    if (productionCoroutine != null)
    //    {
    //        StopCoroutine(productionCoroutine);
    //        productionCoroutine = null;
    //    }

    //}

    //IEnumerator ProduceRiceCake(Collider player)
    //{
    //    yield return new WaitForSeconds(interactionDelay);
    //    while (true)
    //    {
    //        PlayerInventorySO holder = player.GetComponent<PlayerInventorySO>();
    //        if (holder != null)
    //        {
    //            holder.AddItem(tteokItem);
    //        }
    //        yield return new WaitForSeconds(interactionDelay);
    //    }
    //}


    [Header("떡 아이템 설정")]
    public Item tteokItem;
    public float interactionDelay = 1f;               // 플레이어 생산속도
    public float workerProductionInterval = 1.5f;     // 알바생 생산속도

    [Header("임시 저장고 설정")]
    public Transform storageVisualParent;             // 떡 생성 위치
    public int maxStorageCapacity = 15;               // 임시 저장소 최대 개수
    public float stackInterval = 0.8f;                // 생성 떡 간격
    public float PickUpItemTime;                      // 아이템 회수 시간

    [SerializeField]private List<Item> tempStorage = new List<Item>();

    [Header("트리거")]
    public InteractionPoint MakeTrigger;             // 플레이어 생성
    public InteractionPoint temporaryStorageTrigger; // 아이템 회수
    public InteractionPoint UpgradeTrigger;          // 업그레이드 시작

    [Header("업그레이드 설정")]
    public bool isUpgraded = false;
    public int UpgradeCost = 500;                    // 가격
    public float upgradeTime = 5f;                   // 업그레이드 소요 시간
    public GameObject PartTimeWorkerPrefab;

    private Coroutine playerProductionCoroutine;
    private Coroutine transferCoroutine;
    //private Coroutine workerProductionCoroutine;
    //private Coroutine upgradeCoroutine;

    private void Start()
    {
        // 이벤트 연결
        if (MakeTrigger != null)
        {
            MakeTrigger.OnEntered += HandleMakeEnter;
            MakeTrigger.OnExited += HandleMakeExit;
        }
        if (temporaryStorageTrigger != null)
        {
            temporaryStorageTrigger.OnEntered += HandleStorageEnter;
            temporaryStorageTrigger.OnExited += HandleStorageExit;
        }
        if (UpgradeTrigger != null)
        {
            UpgradeTrigger.OnEntered += HandleUpgradeEnter;
            // 업그레이드 중단은 따로 하지 않음
        }

        PartTimeWorkerPrefab.SetActive(false);
    }

    private void OnDisable()
    {
        // 이벤트 해제
        if (MakeTrigger != null)
        {
            MakeTrigger.OnEntered -= HandleMakeEnter;
            MakeTrigger.OnExited -= HandleMakeExit;
        }
        if (temporaryStorageTrigger != null)
        {
            temporaryStorageTrigger.OnEntered -= HandleStorageEnter;
            temporaryStorageTrigger.OnExited -= HandleStorageExit;
        }
        if (UpgradeTrigger != null)
        {
            UpgradeTrigger.OnEntered -= HandleUpgradeEnter;
        }
    }


    private void HandleMakeEnter(Collider other)
    {
        if (playerProductionCoroutine == null)
        {
            playerProductionCoroutine = StartCoroutine(ProduceToStorage(interactionDelay));
        }
    }

    private void HandleMakeExit(Collider other)
    {
        if (playerProductionCoroutine != null)
        {
            StopCoroutine(playerProductionCoroutine);
            playerProductionCoroutine = null;
        }
    }

    private IEnumerator ProduceToStorage(float interval)
    {
        // 첫 생산 대기
        yield return new WaitForSeconds(interval);

        while (true)
        {
            if (tempStorage.Count < maxStorageCapacity)
            {
                tempStorage.Add(tteokItem);
                SpawnStorageVisual();
            }
            yield return new WaitForSeconds(interval);
        }
    }

    private void SpawnStorageVisual()
    {
        if (tteokItem == null || storageVisualParent == null)
            return;

        GameObject prefab = tteokItem.Item_Prefab;
        if (prefab == null)
            return;

        int count = storageVisualParent.childCount;
        Vector3 pos = storageVisualParent.position + Vector3.up * (count * stackInterval);

        Instantiate(prefab, pos, Quaternion.identity, storageVisualParent);
    }

    private void RemoveStorageVisual()
    {
        int count = storageVisualParent.childCount;
        if (count == 0)
            return;

        Transform lastChild = storageVisualParent.GetChild(count - 1);
        Destroy(lastChild.gameObject);
    }

    private void HandleStorageEnter(Collider other)
    {
        if (transferCoroutine == null)
        {
            transferCoroutine = StartCoroutine(TransferToPlayer(other));
        }
    }

    private void HandleStorageExit(Collider other)
    {
        if (transferCoroutine != null)
        {
            StopCoroutine(transferCoroutine);
            transferCoroutine = null;
        }
    }

    private IEnumerator TransferToPlayer(Collider player)
    {
        PlayerInventorySO holder = player.GetComponent<PlayerInventorySO>();
        if (holder == null)
            yield break;

        float delay = PickUpItemTime;
        while (true)
        {
            if (tempStorage.Count > 0)
            {
                Item item = tempStorage[0];
                tempStorage.RemoveAt(0);
                RemoveStorageVisual();

                holder.AddItem(item);
            }
            yield return new WaitForSeconds(delay);
        }
    }

    private void HandleUpgradeEnter(Collider other)
    {
        if (!isUpgraded)
        {
            StartCoroutine(ProcessUpgrade());
        }
    }

    private IEnumerator ProcessUpgrade()
    {
        yield return new WaitForSeconds(upgradeTime);
        if (MoneyManager.Instance.SpendMoney(UpgradeCost))
        {
            isUpgraded = true;
            PartTimeWorkerPrefab.SetActive(true);
            UpgradeTrigger.gameObject.SetActive(false);

            // 자동 생산
            StartCoroutine(ProduceToStorage(workerProductionInterval));
        }
        
    }

}
