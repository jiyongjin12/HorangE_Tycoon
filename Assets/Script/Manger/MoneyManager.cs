using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int currentMoney = 500; // �ʱ� ��
    public static MoneyManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            Debug.Log(amount + "���.����: " + currentMoney);
            return true;
        }
        Debug.Log("��.��!");
        return false;
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        Debug.Log(amount + "ȹ��.��: " + currentMoney);
    }
}
