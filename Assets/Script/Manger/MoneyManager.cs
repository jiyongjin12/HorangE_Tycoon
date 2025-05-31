using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public int Money = 500;
    public static MoneyManager Instance;


    public float animationDuration = 0.7f;
    public TMP_Text moneyText;

    private int currentMoney;
    private Coroutine moneyCoroutine;         

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        currentMoney = Money;
        if (moneyText != null)
            moneyText.text = currentMoney.ToString();
    }

    public bool SpendMoney(int amount)
    {
        if (Money >= amount)
        {
            int previous = Money;
            Money -= amount;

            // 만약 이미 코루틴이 실행 중이라면 멈추고 새로 시작
            if (moneyCoroutine != null)
                StopCoroutine(moneyCoroutine);
            moneyCoroutine = StartCoroutine(AnimateMoneyChange(previous, Money));
            return true;
        }
        return false;
    }

    public void AddMoney(int amount)
    {
        int previous = Money;
        Money += amount;

        if (moneyCoroutine != null)
            StopCoroutine(moneyCoroutine);
        moneyCoroutine = StartCoroutine(AnimateMoneyChange(previous, Money));
    }

    private IEnumerator AnimateMoneyChange(int previous, int target)
    {
        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            // 보간 계산 (부동소수점→정수로 변환)
            float t = Mathf.Clamp01(elapsed / animationDuration);
            int displayed = Mathf.RoundToInt(Mathf.Lerp(previous, target, t));
            currentMoney = displayed;
            if (moneyText != null)
                moneyText.text = currentMoney.ToString();
            yield return null;
        }
        // 마지막에 확실히 target으로 고정
        currentMoney = target;
        if (moneyText != null)
            moneyText.text = currentMoney.ToString();

        // 코루틴 레퍼런스 해제
        moneyCoroutine = null;
    }
}
