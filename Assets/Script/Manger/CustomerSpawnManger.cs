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
    private Transform GetOutPos;

    private int currentCustomerCount;
    public int MaxCustomerCount = 3;

    [SerializeField]
    private float spawnInterval = 3f;
    private float spawnTimer = 0f;

    private void FixedUpdate()
    {
        if (currentCustomerCount < MaxCustomerCount)
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

        currentCustomerCount += 1;
    }

}
