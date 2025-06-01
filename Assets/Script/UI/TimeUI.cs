using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    public Transform handTransform;
    public float test;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(RotateHand(test));
        }
    }

    private IEnumerator RotateHand(float duration)
    {
        float elapsed = 0f;
        float startAngle = 0f;
        float endAngle = -360f;

        while (elapsed < duration)
        {
            float delta = Time.deltaTime;
            elapsed += delta;

            float t = Mathf.Clamp01(elapsed / duration);
            float currentAngle = Mathf.Lerp(startAngle, endAngle, t);

            handTransform.localEulerAngles = new Vector3(0f, 0f, currentAngle);
            Debug.Log(elapsed);

            yield return null;
        }

        // 마지막에 정확하게 초기화
        handTransform.localEulerAngles = new Vector3(0f, 0f, 0f);
    }
}
