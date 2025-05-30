using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawnManger : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _Customer;

    [SerializeField]
    private Transform SpawnPos;

    [SerializeField]
    private float spawnInterval = 3f;
    private float spawnTimer = 0f;

    [SerializeField] private List<GameObject> CustomerList = new List<GameObject>();
    public int MaxCustomerCount = 3;

    private void FixedUpdate()
    {
        // Null 체크 및 리스트 정리
        CustomerList.RemoveAll(customer => customer == null);

        if (CustomerList.Count < MaxCustomerCount)
        {
            spawnTimer += Time.fixedDeltaTime;

            if (spawnTimer >= spawnInterval)
            {
                Spawn();
                spawnTimer = 0f;
            }
        }
    }

    private void Spawn()
    {
        if (_Customer.Count == 0) return;

        GameObject customerPrefab = _Customer[Random.Range(0, _Customer.Count)];
        GameObject newCustomer = Instantiate(customerPrefab, SpawnPos.position, Quaternion.identity);

        // newCustomer.GetComponent<Customer>().SetExit(GetOutPos);

        CustomerList.Add(newCustomer);
    }

}
