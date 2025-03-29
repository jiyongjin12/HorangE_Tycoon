using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int currentMoney = 500; // ÃÊ±â µ·
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
            Debug.Log(amount + "»ç¿ë.³²Àº: " + currentMoney);
            return true;
        }
        Debug.Log("µ·.¾ø!");
        return false;
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        Debug.Log(amount + "È¹µæ.µ·: " + currentMoney);
    }
}
