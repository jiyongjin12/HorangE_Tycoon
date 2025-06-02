using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
    public Transform handTransform;
    public Image fillImage;

    private Coroutine cooldownCoroutine;

    public void StartCooldown(float duration)
    {
        if (cooldownCoroutine != null)
        {
            StopCoroutine(cooldownCoroutine);
        }

        cooldownCoroutine = StartCoroutine(RotateHand(duration));
    }

    public void StopCooldown()
    {
        if (cooldownCoroutine != null)
        {
            StopCoroutine(cooldownCoroutine);
            cooldownCoroutine = null;
        }

        // 상태 초기화
        if (handTransform != null)
            handTransform.localEulerAngles = Vector3.zero;

        if (fillImage != null)
            fillImage.fillAmount = 0f;
    }

    public IEnumerator RotateHand(float duration)
    {
        float elapsed = 0f;
        float startAngle = 0f;
        float endAngle = -360f;

        fillImage.fillAmount = 0f;

        while (elapsed < duration)
        {
            float delta = Time.deltaTime;
            elapsed += delta;

            float t = Mathf.Clamp01(elapsed / duration);
            float currentAngle = Mathf.Lerp(startAngle, endAngle, t);

            handTransform.localEulerAngles = new Vector3(0f, 0f, currentAngle);
            fillImage.fillAmount = t;

            yield return null;
        }

        // 마지막에 정확하게 초기화
        handTransform.localEulerAngles = new Vector3(0f, 0f, 0f);
        fillImage.fillAmount = 0f;
    }
}
